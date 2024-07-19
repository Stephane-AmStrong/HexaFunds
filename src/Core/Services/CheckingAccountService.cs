using DataTransfertObjects;

using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories.Abstractions;

using Mapster;

using Services.Abstractions;

namespace Services;

public sealed class CheckingAccountService(IRepositoryManager repositoryManager) : ICheckingAccountService
{
    public async Task<CheckingAccountResponse> CreateAsync(CheckingAccountRequest checkingAccountRequest, CancellationToken cancellationToken)
    {
        var checkingAccount = checkingAccountRequest.Adapt<CheckingAccount>();

        await repositoryManager.CheckingAccountRepository.CreateAsync(checkingAccount, cancellationToken).ConfigureAwait(false);

        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        return checkingAccount.Adapt<CheckingAccountResponse>();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var checkingAccount = await repositoryManager.CheckingAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        repositoryManager.CheckingAccountRepository.Delete(checkingAccount);

        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public IEnumerable<CheckingAccountResponse> GetAll()
    {
        var checkingAccounts = repositoryManager.CheckingAccountRepository.GetAll();

        return checkingAccounts.Adapt<IEnumerable<CheckingAccountResponse>>();
    }

    public async Task<CheckingAccountResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var checkingAccount = await repositoryManager.CheckingAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        return checkingAccount.Adapt<CheckingAccountResponse>();
    }

    public async Task UpdateAsync(Guid id, CheckingAccountRequest checkingAccountRequest, CancellationToken cancellationToken)
    {
        var checkingAccount = await repositoryManager.CheckingAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        checkingAccountRequest.Balance = checkingAccount.Balance;

        checkingAccountRequest.Adapt(checkingAccount);

        repositoryManager.CheckingAccountRepository.Update(checkingAccount);

        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}
