
using Application.DataTransfertObjects;
using Application.Services.Abstractions;
using Application.UseCases;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories.Abstractions;

using Mapster;


namespace Services;

public sealed class BankAccountService(IBankAccountRepository bankAccountRepository, IUnitOfWork unitOfWork) : IBankAccountService
{
    public async Task<CheckingAccountResponse> CreateAsync(BankAccountRequest bankAccountRequest, CancellationToken cancellationToken)
    {
        var bankAccount = bankAccountRequest.Adapt<BankAccount>();

        await bankAccountRepository.CreateAsync(bankAccount, cancellationToken).ConfigureAwait(false);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return bankAccount.Adapt<CheckingAccountResponse>();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var bankAccount = await bankAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        bankAccountRepository.Delete(bankAccount);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public IList<BankAccountResponse> GetAll()
    {
        var bankAccounts = bankAccountRepository.GetAll();

        return bankAccounts.Adapt<IList<BankAccountResponse>>();
    }

    public async Task<BankAccountResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var bankAccount = await bankAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        return bankAccount.Adapt<BankAccountResponse>();
    }

    public async Task UpdateAsync(Guid id, BankAccountRequest bankAccountRequest, CancellationToken cancellationToken)
    {
        var bankAccount = await bankAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        bankAccountRequest.Adapt(bankAccount);

        bankAccountRepository.Update(bankAccount);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
