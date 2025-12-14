#nullable enable
using Application.UseCases.Alerts.GetByQuery;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.Abstractions.Services;

public interface IAlertsService
{
    Task<PagedList<AlertResponse>> GetPagedListByQueryAsync(AlertQuery query, CancellationToken cancellationToken);
    Task<AlertDetailedResponse?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<AlertResponse> CreateOrIncrementAsync(AlertCreateOrIncrementRequest alertRequest, CancellationToken cancellationToken);
    Task UpdateAsync(string id, AlertUpdateRequest alertRequest, CancellationToken cancellationToken);
    Task DeleteAsync(string id, CancellationToken cancellationToken);
}
