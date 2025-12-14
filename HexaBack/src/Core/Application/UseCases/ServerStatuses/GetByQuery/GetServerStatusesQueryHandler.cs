#nullable enable
using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.ServerStatuses.GetByQuery;

public class GetServerStatusQueryHandler(IServerStatusesService serverStatusesService) : IQueryHandler<GetServerStatusQuery, PagedList<ServerStatusResponse>>
{
    public Task<PagedList<ServerStatusResponse>> HandleAsync(GetServerStatusQuery query, CancellationToken cancellationToken)
    {
        return serverStatusesService.GetPagedListByQueryAsync(new ServerStatusQuery(query.Parameters), cancellationToken);
    }
}
