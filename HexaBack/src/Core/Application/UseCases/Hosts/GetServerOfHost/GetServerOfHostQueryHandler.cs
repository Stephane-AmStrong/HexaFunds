using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Hosts.GetServerOfHost;

public class GetServerOfHostQueryHandler(IServersService serversService) : IQueryHandler<GetServerOfHostQuery, ServerDetailedResponse?>
{
    public Task<ServerDetailedResponse?> HandleAsync(GetServerOfHostQuery query, CancellationToken cancellationToken)
    {
        return serversService.GetByHostAndAppNameAsync(query.HostName, query.AppName, cancellationToken);
    }
}
