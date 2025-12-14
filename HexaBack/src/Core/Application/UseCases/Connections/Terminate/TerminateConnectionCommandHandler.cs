using Application.Abstractions.Handlers;
using Application.Abstractions.Services;

namespace Application.UseCases.Connections.Terminate;

public class TerminateConnectionCommandHandler(IConnectionsService connectionsService) : ICommandHandler<TerminateConnectionCommand>
{
    public Task HandleAsync(TerminateConnectionCommand command, CancellationToken cancellationToken)
    {
        return connectionsService.TerminateAsync(command.Id, cancellationToken);
    }
}