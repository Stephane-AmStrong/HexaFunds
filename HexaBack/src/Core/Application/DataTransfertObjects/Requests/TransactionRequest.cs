using System.Text.Json.Serialization;
using Application.DataTransfertObjects.Enumerations;

namespace Application.DataTransfertObjects.Requests;

public record TransactionRequest
{
    public required float Amount { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransactionType Type { get; init; }

    public Guid AccountId { get; init; }

    [JsonIgnore]
    public DateTime Date { get; init; } = DateTime.UtcNow;
}
