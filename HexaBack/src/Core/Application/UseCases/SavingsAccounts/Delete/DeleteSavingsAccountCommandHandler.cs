using Application.Abstractions.Handlers;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.SavingsAccounts.Delete;

public class DeleteSavingsAccountCommandHandler(ISavingsAccountRepository savingsAccountRepository, IUnitOfWork unitOfWork, ILogger<DeleteSavingsAccountCommandHandler> logger) : ICommandHandler<DeleteSavingsAccountCommand>
{
    public async Task HandleAsync(DeleteSavingsAccountCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting savingsAccount with ID: {SavingsAccountId}", command.Id);

        var savingsAccount = await savingsAccountRepository.GetByIdAsync(command.Id, cancellationToken) ?? throw new AccountNotFoundException(command.Id);

        savingsAccountRepository.Delete(savingsAccount);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully deleted savingsAccount with AccountNumber: {AccountNumber}", savingsAccount.AccountNumber);
    }
}
