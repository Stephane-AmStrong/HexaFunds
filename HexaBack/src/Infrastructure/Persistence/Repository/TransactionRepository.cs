using System.Linq.Expressions;

using Domain.Entities;
using Domain.Abstractions.Repositories;

using Microsoft.EntityFrameworkCore;
using Domain.Shared.Common;
using Persistence.Extensions;

namespace Persistence.Repository;

public sealed class TransactionRepository(BankingDbContext dbContext) : RepositoryBase<Transaction>(dbContext), ITransactionRepository
{
    public Task CreateAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        return BaseCreateAsync(transaction, cancellationToken);
    }

    public void Delete(Transaction transaction)
    {
        BaseDelete(transaction);
    }

    public async Task<IList<Transaction>> FindByConditionAsync(Expression<Func<Transaction, bool>> expression, CancellationToken cancellationToken)
    {
        return await BaseFindByCondition(expression).Include(x=> x.BankAccount).ToListAsync(cancellationToken);
    }

    public IList<Transaction> GetAll()
    {
        return [.. BaseGetAll()];
    }

    public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return BaseFindByCondition(transaction => transaction.Id.Equals(id))
            .Include(x => x.BankAccount)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Transaction>> GetPagedListByQueryAsync(BaseQuery<Transaction> queryParameters, CancellationToken cancellationToken)
    {
        var filterExpression = queryParameters.GetFilterExpression() ?? (x=> true);
        
        var filteredList = BaseFindByCondition(filterExpression);

        if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
        {
            filteredList = filteredList.Include(x=> x.BankAccount).Where(transaction => transaction.BankAccount.AccountNumber.Contains(queryParameters.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        return await filteredList.ApplySorting(queryParameters.OrderBy).ApplyPaging(queryParameters.Page, queryParameters.PageSize, cancellationToken);
    }

    public void Update(Transaction transaction)
    {
        BaseUpdate(transaction);
    }
}
