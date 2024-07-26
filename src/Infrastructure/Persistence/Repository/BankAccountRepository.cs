using Domain.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;
namespace Persistence.Repository;

public sealed class BankAccountRepository(BankingDbContext dbContext) : RepositoryBase<Domain.Entities.BankAccount>(dbContext), IBankAccountRepository
{
    public async Task<Domain.Entities.BankAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await BaseFindByCondition(checkingAccount => checkingAccount.Id.Equals(id))
            .Include(x => x.Transactions)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
