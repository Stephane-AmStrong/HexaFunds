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

    public IList<TransactionResponse> GetAll()
    {
        var transactions = transactionRepository.GetAll();

        return transactions.Adapt<IList<TransactionResponse>>();
    }

    public async Task<TransactionResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await transactionRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new TransactionNotFoundException(id);

        return transaction.Adapt<TransactionResponse>();
    }

    public async Task<AccountTransactionsResponse> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var account = await GetAccount(accountId, cancellationToken).ConfigureAwait(false);

        var transactions = transactionRepository.GetByCondition(transaction => transaction.AccountId == accountId)
            .OrderByDescending(t => t.Date);

        var accountTransactions = new AccountTransactionsResponse
        {
            CheckingAccount = account is CheckingAccount checkingAccount ? checkingAccount.Adapt<CheckingAccountResponse>() : null,
            SavingsAccount = account is SavingsAccount savingsAccount ? savingsAccount.Adapt<SavingsAccountResponse>() : null,
            Transactions = transactions.Adapt<IList<TransactionResponse>>()
        };

        return accountTransactions;
    }


    public async Task<AccountStatementResponse> GetAccountStatementAsync(AccountStatementQuery accountStatementQuery, CancellationToken cancellationToken = default)
    {
        var account = await GetAccount(accountStatementQuery.AccountId, cancellationToken).ConfigureAwait(false);

        var endOfSlidingMonth = accountStatementQuery.StartOfSlidingMonth.AddDays(30);

        var transactions = transactionRepository.GetByCondition(
            transaction => transaction.AccountId == accountStatementQuery.AccountId
            && transaction.Date >= accountStatementQuery.StartOfSlidingMonth
            && transaction.Date <= endOfSlidingMonth
        ).OrderByDescending(t => t.Date);

        var transactionsResponse = transactions.Adapt<TransactionResponse[]>();

        var accountType = GetAccountType(account);

        return new AccountStatementResponse(accountType, account.Balance, transactionsResponse);
    }

    private async Task<Domain.Entities.BankAccount> GetAccount(Guid accountId, CancellationToken cancellationToken)
        => await bankAccountRepository.GetByIdAsync(accountId, cancellationToken).ConfigureAwait(false) ?? throw new TransactionAccountNotFoundException(accountId);


    private readonly Func<Domain.Entities.BankAccount, string> GetAccountType = account => account is CheckingAccount ? STR_CHECKINGACCOUNT : STR_SAVINGSACCOUNT;
}
