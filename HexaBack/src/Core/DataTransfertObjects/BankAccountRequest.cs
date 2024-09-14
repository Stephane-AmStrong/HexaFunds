using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataTransfertObjects;

public abstract record BankAccountRequest
{
    [Required(ErrorMessage = "The AccountNumber is required.")]
    public string? AccountNumber { get; init; }
    [JsonIgnore]
    public float Balance { get; init; }
}
