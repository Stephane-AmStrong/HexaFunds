using Domain.Repositories.Abstractions;

namespace Persistence.Repository;

public sealed class RepositoryManager(BankingDbContext bankingDbContext) : IRepositoryManager
{
    private readonly Lazy<IBankAccountRepository> _lazyBankAccountRepository = new(() => new BankAccountRepository(bankingDbContext));
    private readonly Lazy<ICheckingAccountRepository> _lazyCheckingAccountRepository = new(() => new CheckingAccountRepository(bankingDbContext));
    private readonly Lazy<ISavingsAccountRepository> _lazySavingsAccountRepository = new(() => new SavingsAccountRepository(bankingDbContext));
    private readonly Lazy<ITransactionRepository> _lazyTransactionRepository = new(() => new TransactionRepository(bankingDbContext));
    private readonly Lazy<IUnitOfWork> _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(bankingDbContext));

    public IBankAccountRepository BankAccountRepository => _lazyBankAccountRepository.Value;
    public ICheckingAccountRepository CheckingAccountRepository => _lazyCheckingAccountRepository.Value;
    public ISavingsAccountRepository SavingsAccountRepository => _lazySavingsAccountRepository.Value;
    public ITransactionRepository TransactionRepository => _lazyTransactionRepository.Value;
    public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
}
