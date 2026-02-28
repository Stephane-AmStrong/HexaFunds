using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Responses;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.SavingsAccounts.GetById;

public class GetSavingsAccountByIdQueryHandler(ISavingsAccountRepository savingsAccountRepository, ILogger<GetSavingsAccountByIdQueryHandler> logger) : IQueryHandler<GetSavingsAccountByIdQuery, SavingsAccountResponse?>
{
    public async Task<SavingsAccountResponse?> HandleAsync(GetSavingsAccountByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving savingsAccount with ID: {SavingsAccountId}", query.Id);
        var savingsAccount = await savingsAccountRepository.GetByIdAsync(query.Id, cancellationToken) ?? throw new AccountNotFoundException(query.Id);

        logger.LogInformation("SavingsAccount {AccountNumber} retrieved.", savingsAccount.AccountNumber);

        return savingsAccount.Adapt<SavingsAccountResponse>();
    }
}
