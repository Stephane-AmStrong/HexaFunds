using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Hosts.Create;

public class CreateHostCommandHandler(IHostsService hostsService)
    : ICommandHandler<CreateHostCommand, HostResponse>
{
    public Task<HostResponse> HandleAsync(CreateHostCommand command, CancellationToken cancellationToken)
    {
        return hostsService.CreateAsync(command.Payload, cancellationToken);
    }
}
