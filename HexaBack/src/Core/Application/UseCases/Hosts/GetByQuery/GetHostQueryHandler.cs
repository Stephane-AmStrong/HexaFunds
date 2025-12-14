#nullable enable
using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Hosts.GetByQuery;

public class GetHostQueryHandler(IHostsService hostsService) : IQueryHandler<GetHostQuery, PagedList<HostResponse>>
{
    public Task<PagedList<HostResponse>> HandleAsync(GetHostQuery query, CancellationToken cancellationToken)
    {
        return hostsService.GetPagedListByQueryAsync(new HostQuery(query.Parameters), cancellationToken);
    }
}
