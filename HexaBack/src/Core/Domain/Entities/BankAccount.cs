namespace Domain.Entities;

public class BankAccount
{
    public Guid Id { get; init; }
    public string AccountNumber { get; init; } = string.Empty;
    public IAccountBehavior AccountBehavior { get; private set; } = default!;
    public ICollection<Transaction> Transactions { get; init; } = new HashSet<Transaction>();
    public float Balance { get; set; }

    private AccountBehaviorData BehaviorData { get => AccountBehaviorData.FromDomain(AccountBehavior); set => AccountBehavior = value.ToDomain(); }

    public BankAccount() { }

    public BankAccount(Guid id, string accountNumber, float balance, IAccountBehavior accountBehavior)
    {
        Id = id;
        AccountNumber = accountNumber;
        Balance = balance;
        AccountBehavior = accountBehavior;
    }

    public void ApplyTransaction(Transaction transaction) => AccountBehavior.ApplyTransaction(this, transaction);
}
