using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Hosts.GetByName;

public class GetHostByNameQueryHandler(IHostsService hostsService) : IQueryHandler<GetHostByNameQuery, HostDetailedResponse>
{
    public Task<HostDetailedResponse> HandleAsync(GetHostByNameQuery query, CancellationToken cancellationToken)
    {
        return hostsService.GetByNameAsync(query.Name, cancellationToken);
    }
}
