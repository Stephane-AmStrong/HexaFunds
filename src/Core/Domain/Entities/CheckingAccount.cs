namespace Domain.Entities;

public class CheckingAccount : BankAccount
{
    public float OverdraftLimit { get; init; }
}

