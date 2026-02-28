namespace Domain.Entities;

public abstract class BankAccount : BaseEntity
{
    public BankAccount()
    {
        Transactions = new HashSet<Transaction>();
    }

    public required string AccountNumber { get; init; }
    public float Balance { get; set; }
    public virtual ICollection<Transaction> Transactions { get; init; }
}

