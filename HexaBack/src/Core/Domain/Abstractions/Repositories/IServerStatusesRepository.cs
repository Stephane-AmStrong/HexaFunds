#nullable enable
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Shared.Common;

namespace Domain.Abstractions.Repositories;

public interface IServerStatusesRepository
{
    Task<PagedList<ServerStatus>> GetPagedListByQueryAsync(BaseQuery<ServerStatus> queryParameters, CancellationToken cancellationToken);
    Task<List<ServerStatus>> FindByConditionAsync(Expression<Func<ServerStatus, bool>> expression, CancellationToken cancellationToken);
    Task<List<ServerStatus>> GetLatestByServerIdsAsync(ISet<string> serverIds, CancellationToken cancellationToken);
    Task<ServerStatus?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task CreateAsync(ServerStatus serverStatus, CancellationToken cancellationToken);
}
