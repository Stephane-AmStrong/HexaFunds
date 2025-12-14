using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Servers.GetById;

public class GetServerByIdQueryHandler(IServersService serversService) : IQueryHandler<GetServerByIdQuery, ServerDetailedResponse>
{
    public Task<ServerDetailedResponse> HandleAsync(GetServerByIdQuery query, CancellationToken cancellationToken)
    {
        return serversService.GetByIdAsync(query.Id, cancellationToken);
    }
}
