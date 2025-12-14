#nullable enable
using System.Linq.Expressions;
using Domain.Abstractions.Events;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Shared.Common;
using MongoDB.Driver;

namespace Persistence.Repository;

public sealed class AlertsRepository(IMongoDatabase database, IEventsDispatcher eventsDispatcher) : RepositoryBase<Alert>(database, eventsDispatcher, DataTables.Alerts), IAlertsRepository
{
    public Task<PagedList<Alert>> GetPagedListByQueryAsync(BaseQuery<Alert> queryParameters, CancellationToken cancellationToken)
    {
        return BaseQueryWithFiltersAsync(queryParameters, cancellationToken);
    }

    public Task<List<Alert>> FindByConditionAsync(Expression<Func<Alert, bool>> expression, CancellationToken cancellationToken)
    {
        return BaseFindByConditionAsync(expression, cancellationToken);
    }

    public async Task<Alert?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var alerts = await BaseFindByConditionAsync(alert => alert.Id == id, cancellationToken);
        return alerts.FirstOrDefault();
    }

    public async Task CreateAsync(Alert alert, CancellationToken cancellationToken)
    {
        alert.Raise(new CreatedEvent<Alert>(alert));
        await BaseCreateAsync(alert, cancellationToken);
    }

    public Task UpdateAsync(Alert alert, CancellationToken cancellationToken)
    {
        alert.Raise(new UpdatedEvent<Alert>(alert));
        return BaseUpdateAsync(alert, cancellationToken);
    }

    public Task DeleteAsync(Alert alert, CancellationToken cancellationToken)
    {
        alert.Raise(new DeletedEvent<Alert>(alert));
        return BaseDeleteAsync(alert, cancellationToken);
    }
}
