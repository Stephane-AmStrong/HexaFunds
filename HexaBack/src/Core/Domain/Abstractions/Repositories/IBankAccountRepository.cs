namespace Domain.Abstractions.Repositories;

public interface IBankAccountRepository
{
    Task<Entities.BankAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
