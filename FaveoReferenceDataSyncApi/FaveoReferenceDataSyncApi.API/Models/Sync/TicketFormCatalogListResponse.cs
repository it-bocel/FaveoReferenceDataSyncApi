using System.Text.Json.Serialization;

namespace FaveoReferenceDataSyncApi.API.Models.Sync;

public sealed class TicketFormCatalogListResponse
{
    [JsonPropertyName("data")]
    public List<TicketFormCatalogResponse> Data { get; set; } = [];
}
