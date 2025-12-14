using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Connections.GetById;

public class GetConnectionByIdQueryHandler(IConnectionsService connectionsService) : IQueryHandler<GetConnectionByIdQuery, ConnectionDetailedResponse>
{
    public Task<ConnectionDetailedResponse> HandleAsync(GetConnectionByIdQuery query, CancellationToken cancellationToken)
    {
        return connectionsService.GetByIdAsync(query.Id, cancellationToken);
    }
}
