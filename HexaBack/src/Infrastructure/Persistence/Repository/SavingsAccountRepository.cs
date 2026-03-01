using Domain.Entities;
using Domain.Abstractions.Repositories;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Domain.Shared.Common;
using Persistence.Extensions;
namespace Persistence.Repository;

public sealed class SavingsAccountRepository(BankingDbContext dbContext) : RepositoryBase<SavingsAccount>(dbContext), ISavingsAccountRepository
{
    public Task CreateAsync(SavingsAccount savingsAccount, CancellationToken cancellationToken)
    {
        return BaseCreateAsync(savingsAccount, cancellationToken);
    }

    public void Delete(SavingsAccount savingsAccount)
    {
        BaseDelete(savingsAccount);
    }

    public async Task<IList<SavingsAccount>> FindByConditionAsync(Expression<Func<SavingsAccount, bool>> expression, CancellationToken cancellationToken)
    {
        return await BaseFindByCondition(expression).Include(x=> x.Transactions).ToListAsync(cancellationToken);
    }

    public IList<SavingsAccount> GetAll()
    {
        return [.. BaseGetAll()];
    }

    public Task<SavingsAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return BaseFindByCondition(savingsAccount => savingsAccount.Id.Equals(id))
            .Include(x => x.Transactions)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<SavingsAccount>> GetPagedListByQueryAsync(BaseQuery<SavingsAccount> queryParameters, CancellationToken cancellationToken)
    {
        var filterExpression = queryParameters.GetFilterExpression() ?? (x=> true);
        
        var filteredList = BaseFindByCondition(filterExpression);

        if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
        {
            filteredList = filteredList.Where(savingsAccount => EF.Functions.Like(savingsAccount.AccountNumber, $"%{queryParameters.SearchTerm}%"));
        }

        return await filteredList.Include(x=> x.Transactions).ApplySorting(queryParameters.OrderBy ?? "AccountNumber").ApplyPaging(queryParameters.Page, queryParameters.PageSize, cancellationToken);
    }

    public void Update(SavingsAccount savingsAccount)
    {
        BaseUpdate(savingsAccount);
    }
}
