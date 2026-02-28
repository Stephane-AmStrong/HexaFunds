using Application.Abstractions.Handlers;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CheckingAccounts.Delete;

public class DeleteCheckingAccountCommandHandler(ICheckingAccountRepository checkingAccountRepository, IUnitOfWork unitOfWork, ILogger<DeleteCheckingAccountCommandHandler> logger) : ICommandHandler<DeleteCheckingAccountCommand>
{
    public async Task HandleAsync(DeleteCheckingAccountCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting checkingAccount with ID: {CheckingAccountId}", command.Id);

        var checkingAccount = await checkingAccountRepository.GetByIdAsync(command.Id, cancellationToken) ?? throw new AccountNotFoundException(command.Id);

        checkingAccountRepository.Delete(checkingAccount);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully deleted checkingAccount with AccountNumber: {AccountNumber}", checkingAccount.AccountNumber);
    }
}
