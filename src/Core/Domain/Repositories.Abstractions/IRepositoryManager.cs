namespace Domain.Repositories.Abstractions;

public interface IRepositoryManager
{
    IBankAccountRepository BankAccountRepository { get; }
    ICheckingAccountRepository CheckingAccountRepository { get; }
    ISavingsAccountRepository SavingsAccountRepository { get; }
    ITransactionRepository TransactionRepository { get; }
    IUnitOfWork UnitOfWork { get; }
}
