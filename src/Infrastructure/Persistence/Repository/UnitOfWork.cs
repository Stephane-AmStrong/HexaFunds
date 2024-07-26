using Domain.Repositories.Abstractions;

namespace Persistence.Repository;

public sealed class UnitOfWork(BankingDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
}
