using Application.Abstractions.Handlers;
using Application.Abstractions.Services;

namespace Application.UseCases.Servers.Update;

public class UpdateServerCommandHandler(IServersService serversService) : ICommandHandler<UpdateServerCommand>
{
    public Task HandleAsync(UpdateServerCommand command, CancellationToken cancellationToken)
    {
        return serversService.UpdateAsync(command.Id, command.Payload, cancellationToken);
    }
}
