using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Clients.Create;

public class CreateClientCommandHandler(IClientsService clientsService)
    : ICommandHandler<CreateClientCommand, ClientResponse>
{
    public Task<ClientResponse> HandleAsync(CreateClientCommand command, CancellationToken cancellationToken)
    {
        return clientsService.CreateAsync(command.Payload, cancellationToken);
    }
}
