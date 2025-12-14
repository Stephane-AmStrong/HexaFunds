using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Hosts.GetById;

public class GetHostByIdQueryHandler(IHostsService hostsService) : IQueryHandler<GetHostByIdQuery, HostDetailedResponse>
{
    public Task<HostDetailedResponse> HandleAsync(GetHostByIdQuery query, CancellationToken cancellationToken)
    {
        return hostsService.GetByIdAsync(query.Id, cancellationToken);
    }
}
