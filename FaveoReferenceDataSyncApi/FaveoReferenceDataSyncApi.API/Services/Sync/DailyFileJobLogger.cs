using System.Text;

namespace FaveoReferenceDataSyncApi.API.Services.Sync;

public sealed class DailyFileJobLogger(IWebHostEnvironment environment) : IJobFileLogger
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    public async Task WriteAsync(string message, Exception? exception = null, CancellationToken cancellationToken = default)
    {
        var logsDirectory = Path.Combine(environment.ContentRootPath, "Logs");
        Directory.CreateDirectory(logsDirectory);

        var logPath = Path.Combine(logsDirectory, string.Concat("ticket-form-sync-", DateTime.Now.ToString("yyyy-MM-dd"), ".log"));
        var builder = new StringBuilder()
            .Append("[")
            .Append(DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz"))
            .Append("] ")
            .AppendLine(message);

        if (exception is not null)
        {
            builder.AppendLine(exception.ToString());
        }

        await Semaphore.WaitAsync(cancellationToken);
        try
        {
            await File.AppendAllTextAsync(logPath, builder.ToString(), cancellationToken);
        }
        finally
        {
            Semaphore.Release();
        }
    }
}
