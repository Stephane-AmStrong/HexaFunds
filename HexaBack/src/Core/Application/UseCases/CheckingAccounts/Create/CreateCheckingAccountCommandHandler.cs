using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Responses;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CheckingAccounts.Create;

public class CreateCheckingAccountCommandHandler(ICheckingAccountRepository checkingAccountRepository, IUnitOfWork unitOfWork, ILogger<CreateCheckingAccountCommandHandler> logger) : ICommandHandler<CreateCheckingAccountCommand, CheckingAccountResponse>
{
    public async Task<CheckingAccountResponse> HandleAsync(CreateCheckingAccountCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new checkingAccount with AccountNumber: {AccountNumber} ", command.Payload.AccountNumber);

        var checkingAccount = command.Payload.Adapt<CheckingAccount>();

        await checkingAccountRepository.CreateAsync(checkingAccount, cancellationToken);

        logger.LogInformation("Successfully created checkingAccount with ID: {CheckingAccountId}", checkingAccount.Id);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return checkingAccount.Adapt<CheckingAccountResponse>();

    }
}
