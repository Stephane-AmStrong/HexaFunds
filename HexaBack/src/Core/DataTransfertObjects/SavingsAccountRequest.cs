namespace DataTransfertObjects;

public record SavingsAccountRequest : BankAccountRequest
{

    public required float BalanceCeiling { get; init; }
}
