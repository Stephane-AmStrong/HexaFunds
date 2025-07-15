
using Application.Messagin.Abstractions;

namespace Application.DataTransfertObjects;

public record BankAccountRequest(string? AccountNumber)
{
    protected IAccountBehaviorRequest AccountBehavior { get; private set; } = null!;
}

/*
public record BankAccount(Guid Id, string AccountNumber, IAccountBehavior AccountBehavior, ICollection<Transaction> Transactions)
{
    public float Balance { get; set; }
    public BankAccount(Guid id, string accountNumber, float balance, IAccountBehavior accountBehavior) : this(id, accountNumber, accountBehavior, new HashSet<Transaction>())
    {
        Balance = balance;
    }

    public BankAccount() : this(Guid.Empty, string.Empty, default!, new HashSet<Transaction>())
    { }

    public void ApplyTransaction(Transaction transaction)
    {
        AccountBehavior.ApplyTransaction(this, transaction);
    }
}
*/