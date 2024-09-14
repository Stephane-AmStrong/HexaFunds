namespace Domain.Entities;

public abstract class BankAccount
{
    public BankAccount()
    {
        Transactions = new HashSet<Transaction>();
    }

    public Guid Id { get; init; }
    public required string AccountNumber { get; init; }
    public float Balance { get; set; }
    public virtual ICollection<Transaction> Transactions { get; init; }
}

