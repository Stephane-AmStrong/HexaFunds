using System.Text.Json.Serialization;

namespace DataTransfertObjects;

public record BankAccountRequest
{
    public required string AccountNumber { get; init; }
    [JsonIgnore] 
    public float Balance { get; set; }
}
