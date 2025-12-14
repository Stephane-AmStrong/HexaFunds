#nullable enable
using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Connections.GetByQuery;

public class GetConnectionQueryHandler(IConnectionsService connectionsService) : IQueryHandler<GetConnectionQuery, PagedList<ConnectionResponse>>
{
    public Task<PagedList<ConnectionResponse>> HandleAsync(GetConnectionQuery query, CancellationToken cancellationToken)
    {
        return connectionsService.GetPagedListByQueryAsync(new ConnectionQuery(query.Parameters), cancellationToken);
    }
}
