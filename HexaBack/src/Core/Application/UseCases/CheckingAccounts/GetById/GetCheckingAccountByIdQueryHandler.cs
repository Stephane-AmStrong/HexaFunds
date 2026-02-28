using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Responses;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CheckingAccounts.GetById;

public class GetCheckingAccountByIdQueryHandler(ICheckingAccountRepository checkingAccountRepository, ILogger<GetCheckingAccountByIdQueryHandler> logger) : IQueryHandler<GetCheckingAccountByIdQuery, CheckingAccountResponse?>
{
    public async Task<CheckingAccountResponse?> HandleAsync(GetCheckingAccountByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving checkingAccount with ID: {CheckingAccountId}", query.Id);
        var checkingAccount = await checkingAccountRepository.GetByIdAsync(query.Id, cancellationToken) ?? throw new AccountNotFoundException(query.Id);

        logger.LogInformation("CheckingAccount {AccountNumber} retrieved.", checkingAccount.AccountNumber);

        return checkingAccount.Adapt<CheckingAccountResponse>();
    }
}
