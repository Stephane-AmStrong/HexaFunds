using Domain.Enumerations;
using Domain.Errors;

namespace Domain.Entities;

public record SavingsBehavior(float BalanceCeiling) : IAccountBehavior
{
    public void ApplyTransaction(BankAccount account, Transaction transaction)
    {
        var newBalance = account.Balance + (transaction.Type == TransactionType.Credit ? transaction.Amount : -transaction.Amount);
        if (newBalance > BalanceCeiling) throw new TransactionDepositLimitReachedException(account.Balance, transaction.Amount);
        account.Balance = newBalance;
    }
}
