#nullable enable
using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Hosts.GetWithServersByQuery;

public class GetHostWithServersQueryHandler(IHostsService hostsService) : IQueryHandler<GetHostWithServersQuery, PagedList<HostDetailedResponse>>
{
    public Task<PagedList<HostDetailedResponse>> HandleAsync(GetHostWithServersQuery queryWithServers, CancellationToken cancellationToken)
    {
        return hostsService.GetPagedListWithServersByQueryAsync(new HostWithServersQuery(queryWithServers.Parameters), cancellationToken);
    }
}
