using Application.Abstractions.Services;

using Domain.Entities;
using Domain.Errors;
using Domain.Abstractions.Repositories;

using Mapster;
using Application.UseCases.CheckingAccounts.GetByQuery;
using Application.UseCases.CheckingAccounts.Create;
using Application.UseCases.CheckingAccounts.Update;
using Application.UseCases.CheckingAccounts.GetById;
using Microsoft.Extensions.Logging;

namespace Services;

public sealed class CheckingAccountsService(ICheckingAccountsRepository checkingAccountsRepository, ILogger<CheckingAccountsService> logger, IUnitOfWork unitOfWork) : ICheckingAccountsService
{
    public IList<CheckingAccountResponse> GetAll()
    {
        logger.LogInformation("Retrieving all checking accounts");
        var checkingAccounts = checkingAccountsRepository.GetAll();

        logger.LogInformation("Retrieved {@CheckingAccountsCount} checking", checkingAccounts.Count);
        return checkingAccounts.Adapt<IList<CheckingAccountResponse>>();
    }

    public async Task<CheckingAccountDetailedResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving checking account with ID: {CheckingAccountId}", id);
        var checkingAccount = await checkingAccountsRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        logger.LogInformation("Successfully retrieved checking account {CheckingAccountId}", id);
        return checkingAccount.Adapt<CheckingAccountDetailedResponse>();
    }

    public async Task<CheckingAccountResponse> CreateAsync(CheckingAccountCreateRequest checkingAccountRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new checking account with account number: {AccountNumber}", checkingAccountRequest.AccountNumber);
        var checkingAccount = checkingAccountRequest.Adapt<CheckingAccount>();

        await checkingAccountsRepository.CreateAsync(checkingAccount, cancellationToken).ConfigureAwait(false);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully created checking account with ID: {CheckingAccountId}", checkingAccount.Id);
        return checkingAccount.Adapt<CheckingAccountResponse>();
    }

    public async Task UpdateAsync(Guid id, CheckingAccountUpdateRequest checkingAccountRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating checking account with ID: {CheckingAccountId}", id);
        var checkingAccount = await checkingAccountsRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        checkingAccountRequest = checkingAccountRequest with
        {
            Balance = checkingAccount.Balance
        };

        checkingAccountRequest.Adapt(checkingAccount);

        logger.LogDebug("Applying updates to checking account {CheckingAccountId}: Balance={NewBalance}", id, checkingAccountRequest.Balance);
        checkingAccountsRepository.Update(checkingAccount);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Successfully updated checking account with ID: {CheckingAccountId}", id);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting checking account with ID: {CheckingAccountId}", id);
        var checkingAccount = await checkingAccountsRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        checkingAccountsRepository.Delete(checkingAccount);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Successfully deleted checking account with ID: {CheckingAccountId}", id);
    }
}
