namespace FaveoReferenceDataSyncApi.API.Models;

public sealed class TicketFormApiOptions
{
    public const string SectionName = "TicketFormApi";

    public string? Url { get; set; }

    public string ApiKeyHeaderName { get; set; } = "X-Ticket-Form-Key";

    public string? ApiKey { get; set; }
}
