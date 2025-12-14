using Application.Abstractions.Handlers;
using Application.Abstractions.Services;

namespace Application.UseCases.Clients.Update;

public class UpdateClientCommandHandler(IClientsService clientsService) : ICommandHandler<UpdateClientCommand>
{
    public Task HandleAsync(UpdateClientCommand command, CancellationToken cancellationToken)
    {
        return clientsService.UpdateAsync(command.Id, command.Payload, cancellationToken);
    }
}
