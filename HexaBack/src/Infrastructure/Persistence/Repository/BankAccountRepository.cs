using Domain.Entities;
using Domain.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;
namespace Persistence.Repository;

public sealed class BankAccountRepository(BankingDbContext dbContext) : RepositoryBase<Domain.Entities.BankAccount>(dbContext), IBankAccountRepository
{
    public Task CreateAsync(BankAccount bankAccount, CancellationToken cancellationToken)
    {
        return BaseCreateAsync(bankAccount, cancellationToken);
    }

    public void Delete(BankAccount bankAccount)
    {
        BaseDelete(bankAccount);
    }

    public IList<BankAccount> GetAll()
    {
        return [.. BaseGetAll()];
    }

    public Task<BankAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return BaseFindByCondition(bankAccount => bankAccount.Id.Equals(id))
            .Include(x => x.Transactions)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Update(BankAccount bankAccount)
    {
        BaseUpdate(bankAccount);
    }
}
