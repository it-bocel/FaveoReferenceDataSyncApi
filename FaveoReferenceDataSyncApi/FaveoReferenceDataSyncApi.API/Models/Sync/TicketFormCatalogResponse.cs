using System.Text.Json.Serialization;

namespace FaveoReferenceDataSyncApi.API.Models.Sync;

public sealed class TicketFormCatalogResponse
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
