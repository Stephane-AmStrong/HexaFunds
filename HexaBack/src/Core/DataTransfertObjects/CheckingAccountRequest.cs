using System.ComponentModel.DataAnnotations;

namespace DataTransfertObjects;

public record CheckingAccountRequest : BankAccountRequest
{
    [Required(ErrorMessage = "The OverdraftLimit is required.")]
    [Range(0, float.MaxValue, ErrorMessage = "The OverdraftLimit must be greater than zero.")]
    public required float OverdraftLimit { get; init; }
}
