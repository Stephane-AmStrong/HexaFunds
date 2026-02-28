using System.Linq.Expressions;
using Domain.Entities;
using Domain.Shared.Common;

namespace Domain.Abstractions.Repositories;

public interface ICheckingAccountRepository
{
    Task<PagedList<CheckingAccount>> GetPagedListByQueryAsync(BaseQuery<CheckingAccount> queryParameters, CancellationToken cancellationToken);
    Task<IList<CheckingAccount>> FindByConditionAsync(Expression<Func<CheckingAccount, bool>> expression, CancellationToken cancellationToken);
    Task<CheckingAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(CheckingAccount checkingAccount, CancellationToken cancellationToken);
    void Update(CheckingAccount checkingAccount);
    void Delete(CheckingAccount checkingAccount);
}
