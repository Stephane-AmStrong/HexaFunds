namespace Domain.Entities;

public record SavingsAccount : BankAccount
{
    public float BalanceCeiling { get; init; }
}
