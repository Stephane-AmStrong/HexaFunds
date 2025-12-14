namespace Application.UseCases;

public record SavingsAccountRequest : BankAccountRequest
{

    public required float BalanceCeiling { get; init; }
}
