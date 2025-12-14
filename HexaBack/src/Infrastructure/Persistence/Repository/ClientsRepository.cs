#nullable enable
using System.Linq.Expressions;
using Domain.Abstractions.Events;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Shared.Common;
using MongoDB.Driver;

namespace Persistence.Repository;

public sealed class ClientsRepository(IMongoDatabase database, IEventsDispatcher eventsDispatcher) : RepositoryBase<Client>(database, eventsDispatcher, DataTables.Clients), IClientsRepository
{
    public Task<PagedList<Client>> GetPagedListByQueryAsync(BaseQuery<Client> queryParameters, CancellationToken cancellationToken)
    {
        return BaseQueryWithFiltersAsync(queryParameters, cancellationToken);
    }

    public Task<List<Client>> FindByConditionAsync(Expression<Func<Client, bool>> expression, CancellationToken cancellationToken)
    {
        return BaseFindByConditionAsync(expression, cancellationToken);
    }

    public async Task<Client?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var clients = await BaseFindByConditionAsync(client => client.Id == id, cancellationToken);
        return clients.FirstOrDefault();
    }

    public async Task CreateAsync(Client client, CancellationToken cancellationToken)
    {
        client.Raise(new CreatedEvent<Client>(client));
        await BaseCreateAsync(client, cancellationToken);
    }

    public Task UpdateAsync(Client client, CancellationToken cancellationToken)
    {
        client.Raise(new UpdatedEvent<Client>(client));
        return BaseUpdateAsync(client, cancellationToken);
    }

    public Task DeleteAsync(Client client, CancellationToken cancellationToken)
    {
        client.Raise(new DeletedEvent<Client>(client));
        return BaseDeleteAsync(client, cancellationToken);
    }
}
