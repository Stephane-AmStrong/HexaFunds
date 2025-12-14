#nullable enable
using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Servers.GetByQuery;

public class GetServerQueryHandler(IServersService serversService) : IQueryHandler<GetServerQuery, PagedList<ServerResponse>>
{
    public Task<PagedList<ServerResponse>> HandleAsync(GetServerQuery query, CancellationToken cancellationToken)
    {
        return serversService.GetPagedListByQueryAsync(new ServerQuery(query.Parameters), cancellationToken);
    }
}
