#nullable enable
using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Clients.GetByQuery;

public class GetClientQueryHandler(IClientsService clientsService) : IQueryHandler<GetClientQuery, PagedList<ClientResponse>>
{
    public Task<PagedList<ClientResponse>> HandleAsync(GetClientQuery query, CancellationToken cancellationToken)
    {
        return clientsService.GetPagedListByQueryAsync(new ClientQuery(query.Parameters), cancellationToken);
    }
}
