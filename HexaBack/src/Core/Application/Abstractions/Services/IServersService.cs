#nullable enable
using System.Linq.Expressions;
using Application.UseCases.Servers.GetByQuery;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.Abstractions.Services;

public interface IServersService
{
    Task<PagedList<ServerResponse>> GetPagedListByQueryAsync(ServerQuery query, CancellationToken cancellationToken);
    Task<List<ServerResponse>> FindByConditionAsync(Expression<Func<ServerResponse, bool>> expression, CancellationToken cancellationToken);
    Task<ServerDetailedResponse?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<ServerDetailedResponse?> GetByHostAndAppNameAsync(string hostName, string appName, CancellationToken cancellationToken);
    Task<ServerResponse> CreateAsync(ServerCreateRequest serverRequest, CancellationToken cancellationToken);
    Task UpdateAsync(string id, ServerUpdateRequest serverRequest, CancellationToken cancellationToken);
    Task DeleteAsync(string id, CancellationToken cancellationToken);
}
