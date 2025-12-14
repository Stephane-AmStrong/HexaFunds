using Application.Abstractions.Handlers;
using Application.Abstractions.Services;

namespace Application.UseCases.Hosts.Update;

public class UpdateHostCommandHandler(IHostsService hostsService) : ICommandHandler<UpdateHostCommand>
{
    public Task HandleAsync(UpdateHostCommand command, CancellationToken cancellationToken)
    {
        return hostsService.UpdateAsync(command.Id, command.Payload, cancellationToken);
    }
}
