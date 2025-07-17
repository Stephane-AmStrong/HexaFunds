using System.Text.Json.Serialization;

namespace Application.DataTransfertObjects;

public abstract record BankAccountRequest
{
    public string? AccountNumber { get; init; }
    [JsonIgnore]
    public float Balance { get; init; }
}
