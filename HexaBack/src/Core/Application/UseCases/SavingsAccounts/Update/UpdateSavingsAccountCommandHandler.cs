using Application.Abstractions.Handlers;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.SavingsAccounts.Update;

public class UpdateSavingsAccountCommandHandler(ISavingsAccountRepository savingsAccountRepository, IUnitOfWork unitOfWork, ILogger<UpdateSavingsAccountCommandHandler> logger) : ICommandHandler<UpdateSavingsAccountCommand>
{
    public async Task HandleAsync(UpdateSavingsAccountCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating savingsAccount with ID: {SavingsAccountId}", command.Id);

        var savingsAccount = await savingsAccountRepository.GetByIdAsync(command.Id, cancellationToken) ?? throw new AccountNotFoundException(command.Id);

        command.Payload.Adapt(savingsAccount);

        savingsAccountRepository.Update(savingsAccount);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully updated savingsAccount with AccountNumber: {AccountNumber}", savingsAccount.AccountNumber);
    }
}
