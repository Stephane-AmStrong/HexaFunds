using Domain.Extensions;

namespace Domain.Entities;

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

