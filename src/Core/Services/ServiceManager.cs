using Domain.Repositories.Abstractions;

using Services.Abstractions;

namespace Services;

public sealed class ServiceManager(IRepositoryManager repositoryManager) : IServiceManager
{
    private readonly Lazy<ICheckingAccountService> _lazyCheckingAccountService = new(() => new CheckingAccountService(repositoryManager));
    private readonly Lazy<ISavingsAccountService> _lazySavingsAccountService = new(() => new SavingsAccountService(repositoryManager));
    private readonly Lazy<ITransactionService> _lazyTransactionService = new(() => new TransactionService(repositoryManager));

    public ICheckingAccountService CheckingAccountService => _lazyCheckingAccountService.Value;
    public ISavingsAccountService SavingsAccountService => _lazySavingsAccountService.Value;
    public ITransactionService TransactionService => _lazyTransactionService.Value;
}
