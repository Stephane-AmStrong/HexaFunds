#nullable enable
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Shared.Common;

namespace Domain.Abstractions.Repositories;

public interface IServersRepository
{
    Task<PagedList<Server>> GetPagedListByQueryAsync(BaseQuery<Server> queryParameters, CancellationToken cancellationToken);
    Task<List<Server>> FindByConditionAsync(Expression<Func<Server, bool>> expression, CancellationToken cancellationToken);
    Task<Server?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task CreateAsync(Server server, CancellationToken cancellationToken);
    Task UpdateAsync(Server server, CancellationToken cancellationToken);
    Task DeleteAsync(Server server, CancellationToken cancellationToken);
    Task<long> BulkInsertAsync(ISet<Server> updatedServers, CancellationToken cancellationToken);
    Task<long> BulkUpdateAsync(ISet<string> targetIds, ISet<Server> updatedServers, CancellationToken cancellationToken);
}
