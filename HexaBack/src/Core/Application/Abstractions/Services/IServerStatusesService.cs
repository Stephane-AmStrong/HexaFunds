#nullable enable
using Application.UseCases.ServerStatuses.GetByQuery;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.Abstractions.Services;

public interface IServerStatusesService
{
    Task<PagedList<ServerStatusResponse>> GetPagedListByQueryAsync(ServerStatusQuery query, CancellationToken cancellationToken);
    Task<ServerStatusDetailedResponse?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<ServerStatusResponse> CreateAsync(ServerStatusCreateRequest serverStatusRequest, CancellationToken cancellationToken);
}
