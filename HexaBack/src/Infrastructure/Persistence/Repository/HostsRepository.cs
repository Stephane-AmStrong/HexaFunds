#nullable enable
using System.Linq.Expressions;
using Domain.Abstractions.Events;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Shared.Common;
using MongoDB.Driver;

namespace Persistence.Repository;

public sealed class HostsRepository(IMongoDatabase database, IEventsDispatcher eventsDispatcher) : RepositoryBase<Host>(database, eventsDispatcher, DataTables.Hosts), IHostsRepository
{
    public Task<PagedList<Host>> GetPagedListByQueryAsync(BaseQuery<Host> queryParameters, CancellationToken cancellationToken)
    {
        return BaseQueryWithFiltersAsync(queryParameters, cancellationToken);
    }

    public Task<List<Host>> FindByConditionAsync(Expression<Func<Host, bool>> expression, CancellationToken cancellationToken)
    {
        return BaseFindByConditionAsync(expression, cancellationToken);
    }

    public async Task<Host?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var hosts = await BaseFindByConditionAsync(host => string.Equals(host.Id , id, StringComparison.OrdinalIgnoreCase), cancellationToken);
        return hosts.FirstOrDefault();
    }

    public async Task<Host?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var hosts = await BaseFindByConditionAsync(host => string.Equals(host.Name , name, StringComparison.OrdinalIgnoreCase), cancellationToken);
        return hosts.FirstOrDefault();
    }

    public async Task CreateAsync(Host host, CancellationToken cancellationToken)
    {
        host.Raise(new CreatedEvent<Host>(host));
        await BaseCreateAsync(host, cancellationToken);
    }

    public Task UpdateAsync(Host host, CancellationToken cancellationToken)
    {
        host.Raise(new UpdatedEvent<Host>(host));
        return BaseUpdateAsync(host, cancellationToken);
    }

    public Task DeleteAsync(Host host, CancellationToken cancellationToken)
    {
        host.Raise(new DeletedEvent<Host>(host));
        return BaseDeleteAsync(host, cancellationToken);
    }

    public Task<long> BulkInsertAsync(ISet<Host> createdHosts, CancellationToken cancellationToken)
    {
        foreach (var host in createdHosts)
        {
            host.Raise(new CreatedEvent<Host>(host));
        }

        return BaseBulkOperationsAsync(createdHosts, BulkOperation.Insert, cancellationToken);
    }

    public Task<long> BulkUpdateAsync(ISet<string> targetIds, ISet<Host> updatedHosts, CancellationToken cancellationToken)
    {
        foreach (var host in updatedHosts)
        {
            if (targetIds.Contains(host.Id))
            {
                host.Raise(new UpdatedEvent<Host>(host));
            }
        }

        return BaseBulkOperationsAsync(updatedHosts, BulkOperation.Update, cancellationToken);
    }
}
