using Application.Abstractions.Handlers;
using Application.Abstractions.Services;

namespace Application.UseCases.Clients.Delete;

public class DeleteClientCommandHandler(IClientsService clientsService) : ICommandHandler<DeleteClientCommand>
{
    public Task HandleAsync(DeleteClientCommand command, CancellationToken cancellationToken)
    {
        return clientsService.DeleteAsync(command.Id, cancellationToken);
    }
}
