namespace Services.Abstractions;

public interface IServiceManager
{
    ICheckingAccountService CheckingAccountService { get; }
    ISavingsAccountService SavingsAccountService { get; }
    ITransactionService TransactionService { get; }
}
