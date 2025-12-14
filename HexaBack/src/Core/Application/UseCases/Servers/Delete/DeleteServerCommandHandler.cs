using Application.Abstractions.Handlers;
using Application.Abstractions.Services;

namespace Application.UseCases.Servers.Delete;

public class DeleteServerCommandHandler(IServersService serversService) : ICommandHandler<DeleteServerCommand>
{
    public Task HandleAsync(DeleteServerCommand command, CancellationToken cancellationToken)
    {
        return serversService.DeleteAsync(command.Id, cancellationToken);
    }
}
