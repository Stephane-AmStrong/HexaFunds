#nullable enable
using System.Linq.Expressions;
using Domain.Abstractions.Events;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Shared.Common;
using MongoDB.Driver;

namespace Persistence.Repository;

public sealed class ServerStatusesRepository(IMongoDatabase database, IEventsDispatcher eventsDispatcher) : RepositoryBase<ServerStatus>(database, eventsDispatcher, DataTables.ServerStatuses), IServerStatusesRepository
{
    public Task<PagedList<ServerStatus>> GetPagedListByQueryAsync(BaseQuery<ServerStatus> queryParameters, CancellationToken cancellationToken)
    {
        return BaseQueryWithFiltersAsync(queryParameters, cancellationToken);
    }

    public Task<List<ServerStatus>> FindByConditionAsync(Expression<Func<ServerStatus, bool>> expression, CancellationToken cancellationToken)
    {
        return BaseFindByConditionAsync(expression, cancellationToken);
    }

    public async Task<List<ServerStatus>> GetLatestByServerIdsAsync(ISet<string> serverIds, CancellationToken cancellationToken)
    {
        return await BaseGetLatestByGroupAsync(
            serverStatus => serverIds.Contains(serverStatus.ServerId),
            serverStatus => serverStatus.ServerId,
            serverStatus => serverStatus.RecordedAt,
            cancellationToken);
    }

    public async Task<ServerStatus?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var serverStatuses = await BaseFindByConditionAsync(serverStatus => serverStatus.Id == id, cancellationToken);
        return serverStatuses.FirstOrDefault();
    }

    public async Task CreateAsync(ServerStatus serverStatus, CancellationToken cancellationToken)
    {
        serverStatus.Raise(new CreatedEvent<ServerStatus>(serverStatus));
        await BaseCreateAsync(serverStatus, cancellationToken);
    }
}
