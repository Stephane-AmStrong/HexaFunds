#nullable enable
using Application.UseCases.Connections.GetByQuery;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.Abstractions.Services;

public interface IConnectionsService
{
    Task<PagedList<ConnectionResponse>> GetPagedListByQueryAsync(ConnectionQuery query, CancellationToken cancellationToken);
    Task<ConnectionDetailedResponse?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<ConnectionResponse> EstablishAsync(ConnectionEstablishRequest connectionRequest, CancellationToken cancellationToken);
    Task TerminateAsync(string id, CancellationToken cancellationToken);
}
