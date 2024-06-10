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

        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return savingsAccount.Adapt<SavingsAccountResponse>();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var savingsAccount = await repositoryManager.SavingsAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        await repositoryManager.SavingsAccountRepository.DeleteAsync(savingsAccount, cancellationToken).ConfigureAwait(false);

        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<SavingsAccountResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var savingsAccounts = await repositoryManager.SavingsAccountRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);

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

        await repositoryManager.SavingsAccountRepository.UpdateAsync(savingsAccount, cancellationToken).ConfigureAwait(false);

        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
