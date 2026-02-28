using System.Text.Json.Serialization;

namespace Application.DataTransfertObjects.Requests;

public abstract record BankAccountRequest
{
    public string? AccountNumber { get; init; }
    [JsonIgnore]
    public float Balance { get; init; }
}
