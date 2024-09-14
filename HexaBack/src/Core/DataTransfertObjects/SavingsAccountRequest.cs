using System.ComponentModel.DataAnnotations;

namespace DataTransfertObjects;

public record SavingsAccountRequest : BankAccountRequest
{
    [Required(ErrorMessage = "The BalanceCeiling is required.")]
    [Range(0, float.MaxValue, ErrorMessage = "The BalanceCeiling must be greater than zero.")]
    public required float BalanceCeiling { get; init; }
}
