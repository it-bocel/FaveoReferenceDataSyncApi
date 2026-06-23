using System.Text.Json.Serialization;

namespace FaveoReferenceDataSyncApi.API.Models.Sync;

public sealed class TicketFormEmployeePayload
{
    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = string.Empty;

    [JsonPropertyName("nombreEmpleado")]
    public string NombreEmpleado { get; set; } = string.Empty;

    [JsonPropertyName("fechaIngreso")]
    public string? FechaIngreso { get; set; }

    [JsonPropertyName("cedula")]
    public string? Cedula { get; set; }

    [JsonPropertyName("genero")]
    public string? Genero { get; set; }

    [JsonPropertyName("posicion")]
    public string? Posicion { get; set; }

    [JsonPropertyName("empresa")]
    public string? Empresa { get; set; }

    [JsonPropertyName("localidad")]
    public string? Localidad { get; set; }

    [JsonPropertyName("linea")]
    public string? Linea { get; set; }

    [JsonPropertyName("area")]
    public string? Area { get; set; }

    [JsonPropertyName("seccionDept")]
    public string? SeccionDept { get; set; }

    [JsonPropertyName("estatus")]
    public string? Estatus { get; set; }
}
