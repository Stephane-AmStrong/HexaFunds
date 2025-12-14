using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using Application.UseCases.CheckingAccounts.GetByQuery;

namespace Application.UseCases.CheckingAccounts.Create;

public class CreateCheckingAccountCommandHandler(ICheckingAccountsService checkingaccountsService)
    : ICommandHandler<CreateCheckingAccountCommand, CheckingAccountResponse>
{
    public Task<CheckingAccountResponse> HandleAsync(CreateCheckingAccountCommand command, CancellationToken cancellationToken)
    {
        return checkingaccountsService.CreateAsync(command.Payload, cancellationToken);
    }
}
