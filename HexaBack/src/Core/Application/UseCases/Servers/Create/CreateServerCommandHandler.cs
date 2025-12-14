using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Servers.Create;

public class CreateServerCommandHandler(IServersService serversService)
    : ICommandHandler<CreateServerCommand, ServerResponse>
{
    public Task<ServerResponse> HandleAsync(CreateServerCommand command, CancellationToken cancellationToken)
    {
        return serversService.CreateAsync(command.Payload, cancellationToken);
    }
}