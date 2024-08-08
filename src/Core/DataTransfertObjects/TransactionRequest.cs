using System.Text.Json.Serialization;

namespace DataTransfertObjects;

public record TransactionRequest
{
    public float Amount { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransactionType Type { get; init; }
    public Guid AccountId { get; init; }
    [JsonIgnore]
    public DateTime Date { get; init; } = DateTime.UtcNow;
}
