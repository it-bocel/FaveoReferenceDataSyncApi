using FaveoReferenceDataSyncApi.API.Models.Sync;

namespace FaveoReferenceDataSyncApi.API.Services.Sync;

public interface ITicketFormApiClient
{
    Task<TicketFormCatalogResponse?> SendCatalogAsync(string catalog, string name, CancellationToken cancellationToken);

    Task SendEmployeeAsync(TicketFormEmployeePayload employee, CancellationToken cancellationToken);
}
