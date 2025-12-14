using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Connections.Establish;

public record EstablishConnectionCommandHandler(IConnectionsService ConnectionsService) : ICommandHandler<EstablishConnectionCommand, ConnectionResponse>
{
    public Task<ConnectionResponse> HandleAsync(EstablishConnectionCommand command, CancellationToken cancellationToken)
    {
        return ConnectionsService.EstablishAsync(command.Request, cancellationToken);
    }
}
