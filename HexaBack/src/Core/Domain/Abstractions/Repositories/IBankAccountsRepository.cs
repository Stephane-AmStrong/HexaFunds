namespace Domain.Abstractions.Repositories;

public interface IBankAccountsRepository
{
    Task<Entities.BankAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
