namespace DataTransfertObjects;

public record SavingsAccountRequest : BankAccountRequest
{
    public float BalanceCeiling { get; init; }
}
