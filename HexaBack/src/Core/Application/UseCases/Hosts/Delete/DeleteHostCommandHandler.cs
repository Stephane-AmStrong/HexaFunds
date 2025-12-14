using Application.Abstractions.Handlers;
using Application.Abstractions.Services;

namespace Application.UseCases.Hosts.Delete;

public class DeleteHostCommandHandler(IHostsService hostsService) : ICommandHandler<DeleteHostCommand>
{
    public Task HandleAsync(DeleteHostCommand command, CancellationToken cancellationToken)
    {
        return hostsService.DeleteAsync(command.Id, cancellationToken);
    }
}
