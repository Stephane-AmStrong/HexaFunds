using Application.Abstractions.Handlers;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CheckingAccounts.Update;

public class UpdateCheckingAccountCommandHandler(ICheckingAccountRepository checkingAccountRepository, IUnitOfWork unitOfWork, ILogger<UpdateCheckingAccountCommandHandler> logger) : ICommandHandler<UpdateCheckingAccountCommand>
{
    public async Task HandleAsync(UpdateCheckingAccountCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating checkingAccount with ID: {CheckingAccountId}", command.Id);

        var checkingAccount = await checkingAccountRepository.GetByIdAsync(command.Id, cancellationToken) ?? throw new AccountNotFoundException(command.Id);

        command.Payload.Adapt(checkingAccount);

        checkingAccountRepository.Update(checkingAccount);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully updated checkingAccount with AccountNumber: {AccountNumber}", checkingAccount.AccountNumber);
    }
}
