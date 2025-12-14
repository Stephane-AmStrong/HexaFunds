#nullable enable
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Shared.Common;

namespace Domain.Abstractions.Repositories;

public interface IHostsRepository
{
    Task<PagedList<Host>> GetPagedListByQueryAsync(BaseQuery<Host> queryParameters, CancellationToken cancellationToken);
    Task<List<Host>> FindByConditionAsync(Expression<Func<Host, bool>> expression, CancellationToken cancellationToken);
    Task<Host?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Host?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task CreateAsync(Host host, CancellationToken cancellationToken);
    Task UpdateAsync(Host host, CancellationToken cancellationToken);
    Task DeleteAsync(Host host, CancellationToken cancellationToken);
    Task<long> BulkInsertAsync(ISet<Host> updatedHosts, CancellationToken cancellationToken);
    Task<long> BulkUpdateAsync(ISet<string> targetIds, ISet<Host> updatedHosts, CancellationToken cancellationToken);
}
