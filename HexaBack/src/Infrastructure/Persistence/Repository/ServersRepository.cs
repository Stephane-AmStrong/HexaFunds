#nullable enable
using System.Linq.Expressions;
using Domain.Abstractions.Events;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Shared.Common;
using MongoDB.Driver;

namespace Persistence.Repository;

public sealed class ServersRepository(IMongoDatabase database, IEventsDispatcher eventsDispatcher) : RepositoryBase<Server>(database, eventsDispatcher, DataTables.Servers), IServersRepository
{
    public Task<PagedList<Server>> GetPagedListByQueryAsync(BaseQuery<Server> queryParameters, CancellationToken cancellationToken)
    {
        return BaseQueryWithFiltersAsync(queryParameters, cancellationToken);
    }

    public Task<List<Server>> FindByConditionAsync(Expression<Func<Server, bool>> expression, CancellationToken cancellationToken)
    {
        return BaseFindByConditionAsync(expression, cancellationToken);
    }

    public async Task<Server?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var servers = await BaseFindByConditionAsync(server => server.Id == id, cancellationToken);
        return servers.FirstOrDefault();
    }

    public async Task CreateAsync(Server server, CancellationToken cancellationToken)
    {
        server.Raise(new CreatedEvent<Server>(server));
        await BaseCreateAsync(server, cancellationToken);
    }

    public Task UpdateAsync(Server server, CancellationToken cancellationToken)
    {
        server.Raise(new UpdatedEvent<Server>(server));
        return BaseUpdateAsync(server, cancellationToken);
    }

    public Task DeleteAsync(Server server, CancellationToken cancellationToken)
    {
        server.Raise(new DeletedEvent<Server>(server));
        return BaseDeleteAsync(server, cancellationToken);
    }

    public Task<long> BulkInsertAsync(ISet<Server> createdServers, CancellationToken cancellationToken)
    {
        foreach (var server in createdServers)
        {
            server.Raise(new CreatedEvent<Server>(server));
        }

        return BaseBulkOperationsAsync(createdServers, BulkOperation.Insert, cancellationToken);
    }

    public Task<long> BulkUpdateAsync(ISet<string> targetIds, ISet<Server> updatedServers, CancellationToken cancellationToken)
    {
        foreach (var server in updatedServers)
        {
            if (targetIds.Contains(server.Id))
            {
                server.Raise(new UpdatedEvent<Server>(server));
            }
        }

        return BaseBulkOperationsAsync(updatedServers, BulkOperation.Update, cancellationToken);
    }
}
