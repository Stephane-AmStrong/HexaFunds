using Domain.Enumerations;
using Domain.Errors;

namespace Domain.Entities;

public record CheckingBehavior(float OverdraftLimit) : IAccountBehavior
{
    public void ApplyTransaction(BankAccount account, Transaction transaction)
    {
        var newBalance = account.Balance + (transaction.Type == TransactionType.Credit ? transaction.Amount : -transaction.Amount);
        if (newBalance < -OverdraftLimit) throw new TransactionOverdraftLimitReachedException(account.Balance, transaction.Amount);
        account.Balance = newBalance;
    }
}

