using System.Linq.Expressions;

using Domain.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected RepositoryBase(BankingDbContext dbContext) => BankingDbContext = dbContext;

    protected BankingDbContext BankingDbContext { get; init; }

    public Task BaseCreateAsync(T entity, CancellationToken cancellationToken)
    {
        return BankingDbContext.Set<T>().AddAsync(entity, cancellationToken).AsTask();
    }

    public void BaseDelete(T entity)
    {
        BankingDbContext.Set<T>().Remove(entity);
    }

    public IQueryable<T> BaseFindByCondition(Expression<Func<T, bool>> expression)
    {
        return BankingDbContext.Set<T>().Where(expression).AsNoTracking();
    }

    public IQueryable<T> BaseGetAll()
    {
        return BankingDbContext.Set<T>().AsNoTracking();
    }

    public void BaseUpdate(T entity)
    {
        BankingDbContext.Set<T>().Update(entity);
    }
}
