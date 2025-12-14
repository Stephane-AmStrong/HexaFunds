#nullable enable
using System.Text.Json.Serialization;
using MCS.WatchTower.REST.Application.Enumerations;

namespace Application.UseCases.Alerts.Create;

public record AlertCreateRequest
{
    public string? ServerId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AlertType? Type { get; set; }

    public string? Message { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AlertSeverity? Severity { get; set; }

    public DateTime OccurredAt { get; set; }
}
