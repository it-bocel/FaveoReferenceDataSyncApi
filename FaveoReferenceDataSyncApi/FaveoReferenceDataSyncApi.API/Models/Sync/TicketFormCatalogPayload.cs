using System.Text.Json.Serialization;

namespace FaveoReferenceDataSyncApi.API.Models.Sync;

public sealed class TicketFormCatalogPayload
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
