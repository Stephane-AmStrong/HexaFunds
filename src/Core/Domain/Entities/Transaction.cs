using System.ComponentModel.DataAnnotations.Schema;

using BankAccount.Core.Enumerations;

namespace Domain.Entities;

public record Transaction
{
    public Guid Id { get; init; }
    public float Amount { get; init; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
    public Guid AccountId { get; init; }

    [ForeignKey("AccountId")]
    public virtual required BankAccount BankAccount { get; set; }
};

