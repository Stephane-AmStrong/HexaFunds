namespace Domain.Repositories.Abstractions;

public interface IBankAccountRepository
{
    Task<Entities.BankAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
