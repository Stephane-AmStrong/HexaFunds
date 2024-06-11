namespace Domain.Entities;

public record CheckingAccount : BankAccount
{
    public float OverdraftLimit { get; init; }
}

