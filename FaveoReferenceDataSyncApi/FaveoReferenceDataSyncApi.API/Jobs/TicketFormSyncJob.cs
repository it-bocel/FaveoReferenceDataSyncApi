using FaveoReferenceDataSyncApi.API.Models;
using FaveoReferenceDataSyncApi.API.Models.Sync;
using FaveoReferenceDataSyncApi.API.Services;
using FaveoReferenceDataSyncApi.API.Services.Sync;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FaveoReferenceDataSyncApi.API.Jobs;

public sealed class TicketFormSyncJob(
    IEmployeeCatalogService employeeCatalogService,
    ITicketFormApiClient ticketFormApiClient,
    ISyncCacheStore syncCacheStore,
    IJobFileLogger jobFileLogger,
    ILogger<TicketFormSyncJob> logger) : ITicketFormSyncJob
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task RunAsync()
    {
        var cancellationToken = CancellationToken.None;

        try
        {
            await jobFileLogger.WriteAsync("Ticket form employee sync started.", cancellationToken: cancellationToken);

            var cache = await syncCacheStore.LoadAsync(cancellationToken);
            var employees = await employeeCatalogService.GetEmployeesAsync(cancellationToken);
            var sentEmployees = 0;
            var skippedEmployees = 0;
            var failedEmployees = 0;

            foreach (var employee in employees)
            {
                var payload = TryCreateEmployeePayload(employee);
                if (payload is null)
                {
                    skippedEmployees++;
                    var message = string.Concat("Employee ", employee.Codigo, " was skipped because codigo or nombreEmpleado is missing.");
                    logger.LogWarning("{Message}", message);
                    await jobFileLogger.WriteAsync(message, cancellationToken: cancellationToken);
                    continue;
                }

                var employeeKey = NormalizeKey(payload.Codigo);
                var payloadHash = ComputeHash(payload);

                if (cache.Employees.TryGetValue(employeeKey, out var cachedEmployee)
                    && cachedEmployee.PayloadHash == payloadHash)
                {
                    skippedEmployees++;
                    continue;
                }

                try
                {
                    await ticketFormApiClient.SendEmployeeAsync(payload, cancellationToken);
                    cache.Employees[employeeKey] = new CachedEmployeeItem
                    {
                        PayloadHash = payloadHash,
                        SentAt = DateTimeOffset.UtcNow
                    };
                    sentEmployees++;
                }
                catch (Exception ex)
                {
                    failedEmployees++;
                    var message = string.Concat("Failed to send employee ", payload.Codigo, " - ", payload.NombreEmpleado, ".");
                    logger.LogError(ex, "{Message}", message);
                    await jobFileLogger.WriteAsync(message, ex, cancellationToken);
                }
            }

            await syncCacheStore.SaveAsync(cache, cancellationToken);

            var completedMessage = string.Concat(
                "Ticket form employee sync completed. Sent employees: ", sentEmployees,
                ". Skipped employees: ", skippedEmployees,
                ". Failed employees: ", failedEmployees, ".");

            logger.LogInformation("{Message}", completedMessage);

            await jobFileLogger.WriteAsync(completedMessage, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ticket form employee sync failed with an unexpected error.");
            await jobFileLogger.WriteAsync("Ticket form employee sync failed with an unexpected error.", ex, cancellationToken);
        }
    }

    private static TicketFormEmployeePayload? TryCreateEmployeePayload(EmployeeCatalogItem employee)
    {
        if (string.IsNullOrWhiteSpace(employee.Codigo)
            || string.IsNullOrWhiteSpace(employee.NombreEmpleado))
        {
            return null;
        }

        return new TicketFormEmployeePayload
        {
            Codigo = employee.Codigo.Trim(),
            NombreEmpleado = CleanValue(employee.NombreEmpleado),
            FechaIngreso = employee.FechaIngreso?.ToString("yyyy-MM-ddTHH:mm:ss"),
            Cedula = CleanValue(employee.Cedula),
            Genero = CleanValue(employee.Genero),
            Posicion = CleanValue(employee.Posicion),
            Empresa = employee.Empresa,
            Localidad = CleanValue(employee.Localidad),
            Linea = CleanValue(employee.Linea),
            Area = CleanValue(employee.Area),
            SeccionDept = CleanValue(employee.SeccionDept),
            Estatus = CleanValue(employee.Estatus)
        };
    }

    private static string CleanValue(string? value) => string.Join(" ", (value ?? string.Empty).Split(" ", StringSplitOptions.RemoveEmptyEntries));

    private static string NormalizeKey(string? value) => CleanValue(value).ToUpperInvariant();

    private static string ComputeHash<T>(T value)
    {
        var json = JsonSerializer.Serialize(value, JsonOptions);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(json));
        return Convert.ToHexString(bytes);
    }
}