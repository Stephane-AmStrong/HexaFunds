using System.Text.Json.Serialization;

namespace DataTransfertObjects;

public abstract record BankAccountRequest
{
    public string? AccountNumber { get; init; }
    [JsonIgnore]
    public float Balance { get; init; }
}
