using DataTransfertObjects;

using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories.Abstractions;

using Mapster;

using Services.Abstractions;

namespace Services;

public sealed class SavingsAccountService(IRepositoryManager repositoryManager) : ISavingsAccountService
{
    public async Task<SavingsAccountResponse> CreateAsync(SavingsAccountRequest savingsAccountRequest, CancellationToken cancellationToken = default)
    {
        var savingsAccount = savingsAccountRequest.Adapt<SavingsAccount>();

        await repositoryManager.SavingsAccountRepository.CreateAsync(savingsAccount, cancellationToken).ConfigureAwait(false);

        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        return savingsAccount.Adapt<SavingsAccountResponse>();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var savingsAccount = await repositoryManager.SavingsAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        repositoryManager.SavingsAccountRepository.Delete(savingsAccount);

        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public IEnumerable<SavingsAccountResponse> GetAll()
    {
        var savingsAccounts = repositoryManager.SavingsAccountRepository.GetAll();

        return savingsAccounts.Adapt<IEnumerable<SavingsAccountResponse>>();
    }

    public async Task<SavingsAccountResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var savingsAccount = await repositoryManager.SavingsAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        return savingsAccount.Adapt<SavingsAccountResponse>();
    }

    public async Task UpdateAsync(Guid id, SavingsAccountRequest savingsAccountRequest, CancellationToken cancellationToken = default)
    {
        var savingsAccount = await repositoryManager.SavingsAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        savingsAccountRequest.Balance = savingsAccount.Balance;

        savingsAccountRequest.Adapt(savingsAccount);

        repositoryManager.SavingsAccountRepository.Update(savingsAccount);

        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}
