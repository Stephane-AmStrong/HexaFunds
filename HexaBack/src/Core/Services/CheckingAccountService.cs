using DataTransfertObjects;

using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories.Abstractions;

using Mapster;

using Services.Abstractions;
using Services.Utils;

namespace Services;

public sealed class CheckingAccountService(ICheckingAccountRepository checkingAccountRepository, IUnitOfWork unitOfWork) : ICheckingAccountService
{
    public async Task<CheckingAccountResponse> CreateAsync(CheckingAccountRequest checkingAccountRequest, CancellationToken cancellationToken)
    {
        var validationResults = CustomValidator.ValidateModel(checkingAccountRequest);
        if (validationResults.Any()) throw new BadRequestException(string.Join(' ', validationResults.Select(x => x.Value)));

        var checkingAccount = checkingAccountRequest.Adapt<CheckingAccount>();

        await checkingAccountRepository.CreateAsync(checkingAccount, cancellationToken).ConfigureAwait(false);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return checkingAccount.Adapt<CheckingAccountResponse>();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var checkingAccount = await checkingAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        checkingAccountRepository.Delete(checkingAccount);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public IList<CheckingAccountResponse> GetAll()
    {
        var checkingAccounts = checkingAccountRepository.GetAll();

        return checkingAccounts.Adapt<IList<CheckingAccountResponse>>();
    }

    public async Task<CheckingAccountResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var checkingAccount = await checkingAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        return checkingAccount.Adapt<CheckingAccountResponse>();
    }

    public async Task UpdateAsync(Guid id, CheckingAccountRequest checkingAccountRequest, CancellationToken cancellationToken)
    {
        var validationResults = CustomValidator.ValidateModel(checkingAccountRequest);
        if (validationResults.Any()) throw new BadRequestException(string.Join(' ', validationResults.Select(x => x.Value)));

        var checkingAccount = await checkingAccountRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AccountNotFoundException(id);

        checkingAccountRequest = checkingAccountRequest with
        {
            Balance = checkingAccount.Balance
        };

        checkingAccountRequest.Adapt(checkingAccount);

        checkingAccountRepository.Update(checkingAccount);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
