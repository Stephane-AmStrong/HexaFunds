using System.Text.Json.Serialization;

namespace DataTransfertObjects;

public record TransactionRequest
{
    public float Amount { get; init; }
    public TransactionType Type { get; set; }
    public Guid AccountId { get; init; }
}
