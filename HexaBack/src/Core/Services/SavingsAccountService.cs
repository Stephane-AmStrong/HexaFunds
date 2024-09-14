using DataTransfertObjects;

using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories.Abstractions;

using Mapster;

using Services.Abstractions;
using Services.Utils;

namespace Services;

public sealed class SavingsAccountService(ISavingsAccountRepository savingsAccountRepository, IUnitOfWork unitOfWork) : ISavingsAccountService
{
    public async Task<SavingsAccountResponse> CreateAsync(SavingsAccountRequest savingsAccountRequest, CancellationToken cancellationToken = default)
    {
        var validationResults = CustomValidator.ValidateModel(savingsAccountRequest);
        if (validationResults.Any()) throw new BadRequestException(string.Join(' ', validationResults.Select(x => x.Value)));

        var savingsAccount = savingsAccountRequest.Adapt<SavingsAccount>();

        await savingsAccountRepository.CreateAsync(savingsAccount, cancellationToken).ConfigureAwait(false);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return savingsAccount.Adapt<SavingsAccountResponse>();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var savingsAccount = await savingsAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        savingsAccountRepository.Delete(savingsAccount);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public IList<SavingsAccountResponse> GetAll()
    {
        var savingsAccounts = savingsAccountRepository.GetAll();

        return savingsAccounts.Adapt<IList<SavingsAccountResponse>>();
    }

    public async Task<SavingsAccountResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var savingsAccount = await savingsAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        return savingsAccount.Adapt<SavingsAccountResponse>();
    }

    public async Task UpdateAsync(Guid id, SavingsAccountRequest savingsAccountRequest, CancellationToken cancellationToken = default)
    {
        var validationResults = CustomValidator.ValidateModel(savingsAccountRequest);
        if (validationResults.Any()) throw new BadRequestException(string.Join(' ', validationResults.Select(x => x.Value)));

        var savingsAccount = await savingsAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        savingsAccountRequest = savingsAccountRequest with
        {
            Balance = savingsAccount.Balance
        };

        savingsAccountRequest.Adapt(savingsAccount);

        savingsAccountRepository.Update(savingsAccount);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
