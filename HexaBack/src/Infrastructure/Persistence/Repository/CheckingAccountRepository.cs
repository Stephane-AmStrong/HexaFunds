using Domain.Entities;
using Domain.Abstractions.Repositories;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Domain.Shared.Common;
using Persistence.Extensions;
namespace Persistence.Repository;

public sealed class CheckingAccountRepository(BankingDbContext dbContext) : RepositoryBase<CheckingAccount>(dbContext), ICheckingAccountRepository
{
    public Task CreateAsync(CheckingAccount checkingAccount, CancellationToken cancellationToken)
    {
        return BaseCreateAsync(checkingAccount, cancellationToken);
    }

    public void Delete(CheckingAccount checkingAccount)
    {
        BaseDelete(checkingAccount);
    }

    public async Task<IList<CheckingAccount>> FindByConditionAsync(Expression<Func<CheckingAccount, bool>> expression, CancellationToken cancellationToken)
    {
        return await BaseFindByCondition(expression).Include(x=> x.Transactions).ToListAsync(cancellationToken);
    }

    public Task<CheckingAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return BaseFindByCondition(checkingAccount => checkingAccount.Id.Equals(id))
            .Include(x => x.Transactions)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<CheckingAccount>> GetPagedListByQueryAsync(BaseQuery<CheckingAccount> queryParameters, CancellationToken cancellationToken)
    {
        var filterExpression = queryParameters.GetFilterExpression() ?? (x=> true);
        
        var filteredList = BaseFindByCondition(filterExpression);

        if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
        {
            filteredList = filteredList.Where(checkingAccount => EF.Functions.Like(checkingAccount.AccountNumber, $"%{queryParameters.SearchTerm}%"));
        }

        return await filteredList.Include(x=> x.Transactions).ApplySorting(queryParameters.OrderBy ?? "AccountNumber").ApplyPaging(queryParameters.Page, queryParameters.PageSize, cancellationToken);
    }

    public void Update(CheckingAccount checkingAccount)
    {
        BaseUpdate(checkingAccount);
    }
}
