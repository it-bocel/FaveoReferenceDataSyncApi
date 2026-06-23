using System.Text.Json.Serialization;

namespace FaveoReferenceDataSyncApi.API.Models.Sync;

public sealed class TicketFormCatalogSingleResponse
{
    [JsonPropertyName("data")]
    public TicketFormCatalogResponse? Data { get; set; }
}
