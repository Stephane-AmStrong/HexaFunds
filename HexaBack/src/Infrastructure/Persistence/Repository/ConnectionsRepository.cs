#nullable enable
using System.Linq.Expressions;
using Domain.Abstractions.Events;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Shared.Common;
using MongoDB.Driver;

namespace Persistence.Repository;

public sealed class ConnectionsRepository(IMongoDatabase database, IEventsDispatcher eventsDispatcher) : RepositoryBase<Connection>(database, eventsDispatcher, DataTables.Connections), IConnectionsRepository
{
    public Task<PagedList<Connection>> GetPagedListByQueryAsync(BaseQuery<Connection> queryParameters, CancellationToken cancellationToken)
    {
        return BaseQueryWithFiltersAsync(queryParameters, cancellationToken);
    }

    public Task<List<Connection>> FindByConditionAsync(Expression<Func<Connection, bool>> expression, CancellationToken cancellationToken)
    {
        return BaseFindByConditionAsync(expression, cancellationToken);
    }

    public async Task<Connection?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var connections = await BaseFindByConditionAsync(connection => connection.Id == id, cancellationToken);
        return connections.FirstOrDefault();
    }

    public async Task EstablishAsync(Connection connection, CancellationToken cancellationToken)
    {
        connection.Raise(new CreatedEvent<Connection>(connection));
        await BaseCreateAsync(connection, cancellationToken);
    }

    public Task TerminateAsync(Connection connection, CancellationToken cancellationToken)
    {
        connection.Raise(new DeletedEvent<Connection>(connection));
        return BaseDeleteAsync(connection, cancellationToken);
    }
}
