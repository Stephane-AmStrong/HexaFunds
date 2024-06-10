using DataTransfertObjects;

using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories.Abstractions;

using Mapster;

using Services.Abstractions;

namespace Services;

public sealed class CheckingAccountService(IRepositoryManager repositoryManager) : ICheckingAccountService
{
    public async Task<CheckingAccountResponse> CreateAsync(CheckingAccountRequest checkingAccountRequest, CancellationToken cancellationToken = default)
    {
        var checkingAccount = checkingAccountRequest.Adapt<CheckingAccount>();

        await repositoryManager.CheckingAccountRepository.CreateAsync(checkingAccount, cancellationToken).ConfigureAwait(false);

        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return checkingAccount.Adapt<CheckingAccountResponse>();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var checkingAccount = await repositoryManager.CheckingAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        await repositoryManager.CheckingAccountRepository.DeleteAsync(checkingAccount, cancellationToken).ConfigureAwait(false);

        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<CheckingAccountResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var checkingAccounts = await repositoryManager.CheckingAccountRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);

        return checkingAccounts.Adapt<IEnumerable<CheckingAccountResponse>>();
    }

    public async Task<CheckingAccountResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var checkingAccount = await repositoryManager.CheckingAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        return checkingAccount.Adapt<CheckingAccountResponse>();
    }

    public async Task UpdateAsync(Guid id, CheckingAccountRequest checkingAccountRequest, CancellationToken cancellationToken = default)
    {
        var checkingAccount = await repositoryManager.CheckingAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        checkingAccountRequest.Balance = checkingAccount.Balance;

        checkingAccountRequest.Adapt(checkingAccount);

        await repositoryManager.CheckingAccountRepository.UpdateAsync(checkingAccount, cancellationToken).ConfigureAwait(false);

        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
