using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Responses;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.SavingsAccounts.Create;

public class CreateSavingsAccountCommandHandler(ISavingsAccountRepository savingsAccountRepository, IUnitOfWork unitOfWork, ILogger<CreateSavingsAccountCommandHandler> logger) : ICommandHandler<CreateSavingsAccountCommand, SavingsAccountResponse>
{
    public async Task<SavingsAccountResponse> HandleAsync(CreateSavingsAccountCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new savingsAccount with AccountNumber: {AccountNumber} ", command.Payload.AccountNumber);

        var savingsAccount = command.Payload.Adapt<SavingsAccount>();

        await savingsAccountRepository.CreateAsync(savingsAccount, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully created savingsAccount with ID: {SavingsAccountId}", savingsAccount.Id);

        return savingsAccount.Adapt<SavingsAccountResponse>();

    }
}
