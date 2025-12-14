using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Clients.GetById;

public class GetClientByIdQueryHandler(IClientsService clientsService) : IQueryHandler<GetClientByIdQuery, ClientDetailedResponse>
{
    public Task<ClientDetailedResponse> HandleAsync(GetClientByIdQuery query, CancellationToken cancellationToken)
    {
        return clientsService.GetByIdAsync(query.Id, cancellationToken);
    }
}
