using Application.Abstractions.Handlers;
using Application.Abstractions.Services;

namespace Application.UseCases.CheckingAccounts.Update;

public class UpdateCheckingAccountCommandHandler(ICheckingAccountsService checkingaccountsService) : ICommandHandler<UpdateCheckingAccountCommand>
{
    public Task HandleAsync(UpdateCheckingAccountCommand command, CancellationToken cancellationToken)
    {
        return checkingaccountsService.UpdateAsync(command.Id, command.Payload, cancellationToken);
    }
}
