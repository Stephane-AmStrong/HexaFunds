using System.Linq.Expressions;
using Domain.Entities;
using Domain.Shared.Common;

namespace Domain.Abstractions.Repositories;

public interface ITransactionRepository
{
    Task<PagedList<Transaction>> GetPagedListByQueryAsync(BaseQuery<Transaction> queryParameters, CancellationToken cancellationToken);
    Task<IList<Transaction>> FindByConditionAsync(Expression<Func<Transaction, bool>> expression, CancellationToken cancellationToken);
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(Transaction transaction, CancellationToken cancellationToken);
    void Update(Transaction transaction);
    void Delete(Transaction transaction);
}
