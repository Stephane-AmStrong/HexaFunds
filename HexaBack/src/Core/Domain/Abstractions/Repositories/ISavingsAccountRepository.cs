using System.Linq.Expressions;
using Domain.Entities;
using Domain.Shared.Common;

namespace Domain.Abstractions.Repositories;

public interface ISavingsAccountRepository
{
    Task<PagedList<SavingsAccount>> GetPagedListByQueryAsync(BaseQuery<SavingsAccount> queryParameters, CancellationToken cancellationToken);
    Task<IList<SavingsAccount>> FindByConditionAsync(Expression<Func<SavingsAccount, bool>> expression, CancellationToken cancellationToken);
    Task<SavingsAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(SavingsAccount savingsAccount, CancellationToken cancellationToken);
    void Update(SavingsAccount savingsAccount);
    void Delete(SavingsAccount savingsAccount);
}
