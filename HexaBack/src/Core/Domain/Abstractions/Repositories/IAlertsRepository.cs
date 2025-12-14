#nullable enable
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Shared.Common;

namespace Domain.Abstractions.Repositories;

public interface IAlertsRepository
{
    Task<PagedList<Alert>> GetPagedListByQueryAsync(BaseQuery<Alert> queryParameters, CancellationToken cancellationToken);
    Task<List<Alert>> FindByConditionAsync(Expression<Func<Alert, bool>> expression, CancellationToken cancellationToken);
    Task<Alert?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task CreateAsync(Alert alert, CancellationToken cancellationToken);
    Task UpdateAsync(Alert alert, CancellationToken cancellationToken);
    Task DeleteAsync(Alert alert, CancellationToken cancellationToken);
}
