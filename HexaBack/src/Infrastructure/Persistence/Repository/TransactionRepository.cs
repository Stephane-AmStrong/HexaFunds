using System.Linq.Expressions;

using Domain.Entities;
using Domain.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository;

public sealed class TransactionRepository(BankingDbContext dbContext) : RepositoryBase<Transaction>(dbContext), ITransactionRepository
{
    public async Task CreateAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        await BaseCreateAsync(transaction, cancellationToken);
    }

    public void Delete(Transaction transaction)
    {
        BaseDelete(transaction);
    }

    public IQueryable<Transaction> GetAll()
    {
        return BaseGetAll();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await BaseFindByCondition(transaction => transaction.Id.Equals(id))
            .Include(x => x.BankAccount)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public IQueryable<Transaction> GetByCondition(Expression<Func<Transaction, bool>> expression)
    {
        return BaseFindByCondition(expression)
            .Include(x => x.BankAccount);
    }

    public void Update(Transaction transaction)
    {
        BaseUpdate(transaction);
    }
}
