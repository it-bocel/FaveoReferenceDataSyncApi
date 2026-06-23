using FaveoReferenceDataSyncApi.API.Models.Sync;
using System.Net.Http.Json;

namespace FaveoReferenceDataSyncApi.API.Services.Sync;

public sealed class TicketFormApiClient(IHttpClientFactory httpClientFactory) : ITicketFormApiClient
{
    public async Task<TicketFormCatalogResponse?> SendCatalogAsync(string catalog, string name, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("TicketFormApi");
        var endpoint = string.Concat("catalogs/", catalog);
        using var response = await client.PostAsJsonAsync(endpoint, new TicketFormCatalogPayload { Name = name }, cancellationToken);
        await EnsureSuccessAsync(response, endpoint, cancellationToken);

        if (response.Content.Headers.ContentLength == 0)
        {
            return null;
        }

        var wrappedResponse = await response.Content.ReadFromJsonAsync<TicketFormCatalogSingleResponse>(cancellationToken);
        return wrappedResponse?.Data;
    }

    public async Task SendEmployeeAsync(TicketFormEmployeePayload employee, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("TicketFormApi");
        const string endpoint = "employees";
        using var response = await client.PostAsJsonAsync(endpoint, employee, cancellationToken);
        await EnsureSuccessAsync(response, endpoint, cancellationToken);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, string endpoint, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new HttpRequestException(
            string.Concat(
                "Remote API request failed. Endpoint: ", endpoint,
                ". Status: ", (int)response.StatusCode,
                " ", response.ReasonPhrase,
                ". Body: ", body),
            null,
            response.StatusCode);
    }
}
