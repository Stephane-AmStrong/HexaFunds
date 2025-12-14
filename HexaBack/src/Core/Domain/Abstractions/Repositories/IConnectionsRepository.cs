#nullable enable
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Shared.Common;

namespace Domain.Abstractions.Repositories;

public interface IConnectionsRepository
{
    Task<PagedList<Connection>> GetPagedListByQueryAsync(BaseQuery<Connection> queryParameters, CancellationToken cancellationToken);
    Task<List<Connection>> FindByConditionAsync(Expression<Func<Connection, bool>> expression, CancellationToken cancellationToken);
    Task<Connection?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task EstablishAsync(Connection connection, CancellationToken cancellationToken);
    Task TerminateAsync(Connection connection, CancellationToken cancellationToken);
}
