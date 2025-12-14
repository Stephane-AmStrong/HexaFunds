using Application.UseCases.CheckingAccounts.Create;
using Application.UseCases.CheckingAccounts.GetById;
using Application.UseCases.CheckingAccounts.GetByQuery;
using Application.UseCases.CheckingAccounts.Update;

namespace Application.Abstractions.Services;

public interface ICheckingAccountsService
{
    IList<CheckingAccountResponse> GetAll();
    Task<CheckingAccountDetailedResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<CheckingAccountResponse> CreateAsync(CheckingAccountCreateRequest checkingAccount, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, CheckingAccountUpdateRequest checkingAccount, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
