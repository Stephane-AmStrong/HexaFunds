using Application.Abstractions.Handlers;
using Application.Abstractions.Services;

namespace Application.UseCases.CheckingAccounts.Delete;

public class DeleteCheckingAccountCommandHandler(ICheckingAccountsService checkingaccountsService) : ICommandHandler<DeleteCheckingAccountCommand>
{
    public Task HandleAsync(DeleteCheckingAccountCommand command, CancellationToken cancellationToken)
    {
        return checkingaccountsService.DeleteAsync(command.Id, cancellationToken);
    }
}
