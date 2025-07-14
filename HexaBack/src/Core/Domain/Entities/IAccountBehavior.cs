using System;

namespace Domain.Entities;

public interface IAccountBehavior
{
    void ApplyTransaction(BankAccount account, Transaction transaction);
}
