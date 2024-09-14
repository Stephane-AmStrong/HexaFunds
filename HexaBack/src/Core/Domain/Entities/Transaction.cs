using System.ComponentModel.DataAnnotations.Schema;

using BankAccount.Core.Enumerations;

namespace Domain.Entities;

public class Transaction
{
    public Guid Id { get; init; }
    public float Amount { get; init; }
    public TransactionType Type { get; init; }
    public DateTime Date { get; init; }
    public Guid AccountId { get; init; }

    [ForeignKey("AccountId")]
    public virtual required BankAccount BankAccount { get; init; }
};

