using Application.DataTransfertObjects;
using Application.Messagin.Abstractions;
using Application.Services.Abstractions;

namespace Application.UseCases;

public record CreateCheckingAccountHandler(IBankAccountService Service) : ICommandHandler<CreateCheckingAccountCommand, CheckingAccountResponse>
{
    public async ValueTask<CheckingAccountResponse> HandleAsync(CreateCheckingAccountCommand command, CancellationToken cancellationToken = default)
    {
        return await Service.CreateAsync(command, cancellationToken);
    }
}
