using DataTransfertObjects;

using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories.Abstractions;

using Mapster;

using Services.Abstractions;
using Services.Utils;

namespace Services;

public sealed class TransactionService(
    ITransactionRepository transactionRepository,
    ICheckingAccountRepository checkingAccountRepository,
    ISavingsAccountRepository savingsAccountRepository,
    IBankAccountRepository bankAccountRepository,
    IUnitOfWork unitOfWork
    ) : ITransactionService
{
    private const string STR_CHECKINGACCOUNT = "Compte Courant";
    private const string STR_SAVINGSACCOUNT = "Livret ";

    public async Task<TransactionResponse> CreateAsync(TransactionRequest transactionRequest, CancellationToken cancellationToken = default)
    {
        var validationResults = CustomValidator.ValidateModel(transactionRequest);
        if (validationResults.Any()) throw new BadRequestException(string.Join(' ', validationResults.Select(x => x.Value)));

        var account = await GetAccount(transactionRequest.AccountId, cancellationToken).ConfigureAwait(false);

        var nextBalance = transactionRequest.Type == TransactionType.Credit ? account.Balance + transactionRequest.Amount : account.Balance - transactionRequest.Amount;

        var transaction = transactionRequest.Adapt<Transaction>();

        if (account is CheckingAccount checkingAccount)
        {
            if (nextBalance < 0 && Math.Abs(nextBalance) > checkingAccount.OverdraftLimit) throw new TransactionOverdraftLimitReachedException(checkingAccount.Balance, transactionRequest.Amount);

            checkingAccount.Balance = nextBalance;

            await transactionRepository.CreateAsync(transaction, cancellationToken).ConfigureAwait(false);

            checkingAccountRepository.Update(checkingAccount);
        }

        if (account is SavingsAccount savingsAccount)
        {
            if (nextBalance < 0) throw new TransactionWithdrawalExceedException();

            if (nextBalance > savingsAccount.BalanceCeiling) throw new TransactionDepositLimitReachedException(savingsAccount.Balance, transactionRequest.Amount);

            savingsAccount.Balance = nextBalance;

            await transactionRepository.CreateAsync(transaction, cancellationToken).ConfigureAwait(false);

            savingsAccountRepository.Update(savingsAccount);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return transaction.Adapt<TransactionResponse>();
    }

    public IList<TransactionResponse> Get(TransactionQuery transactionQuery)
    {
        var transactions = transactionRepository.GetAll();

        if (transactionQuery.WithAccountId is not null)
        {
            transactions = transactions.Where(x => x.AccountId == transactionQuery.WithAccountId);
        }

        if (transactionQuery.FromDate is not null)
        {
            transactions = transactions.Where(x => x.Date >= transactionQuery.FromDate);
        }

        if (transactionQuery.ToDate is not null)
        {
            transactions = transactions.Where(x => x.Date <= transactionQuery.ToDate);
        }

        return transactions.Adapt<IList<TransactionResponse>>();
    }

    public async Task<TransactionResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await transactionRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new TransactionNotFoundException(id);

        return transaction.Adapt<TransactionResponse>();
    }

    private async Task<Domain.Entities.BankAccount> GetAccount(Guid accountId, CancellationToken cancellationToken)
        => await bankAccountRepository.GetByIdAsync(accountId, cancellationToken).ConfigureAwait(false) ?? throw new TransactionAccountNotFoundException(accountId);


    private readonly Func<Domain.Entities.BankAccount, string> GetAccountType = account => account is CheckingAccount ? STR_CHECKINGACCOUNT : STR_SAVINGSACCOUNT;
}
