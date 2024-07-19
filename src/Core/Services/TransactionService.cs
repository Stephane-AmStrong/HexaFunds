using DataTransfertObjects;

using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories.Abstractions;

using Mapster;

using Services.Abstractions;

namespace Services;

public sealed class TransactionService(IRepositoryManager repositoryManager) : ITransactionService
{
    private const string STR_CHECKINGACCOUNT = "Compte Courant";
    private const string STR_SAVINGSACCOUNT = "Livret ";

    public async Task<TransactionResponse> CreateAsync(TransactionRequest transactionRequest, CancellationToken cancellationToken = default)
    {
        CheckTransactionAmount(transactionRequest.Amount);

        var account = await GetAccount(repositoryManager, transactionRequest.AccountId, cancellationToken).ConfigureAwait(false);

        var nextBalance = transactionRequest.Type == TransactionType.Credit ? account.Balance + transactionRequest.Amount : account.Balance - transactionRequest.Amount;

        var transaction = transactionRequest.Adapt<Transaction>();
        transaction.Date = DateTime.UtcNow;

        if (account is CheckingAccount checkingAccount)
        {
            if (nextBalance < 0 && Math.Abs(nextBalance) > checkingAccount.OverdraftLimit) throw new TransactionOverdraftLimitReachedException(checkingAccount.Balance, transactionRequest.Amount);

            checkingAccount.Balance = nextBalance;

            await repositoryManager.TransactionRepository.CreateAsync(transaction, cancellationToken).ConfigureAwait(false);

            repositoryManager.CheckingAccountRepository.Update(checkingAccount);
        }

        if (account is SavingsAccount savingsAccount)
        {
            if (nextBalance < 0) throw new TransactionWithdrawalExceedException();

            if (nextBalance > savingsAccount.BalanceCeiling) throw new TransactionDepositLimitReachedException(savingsAccount.Balance, transactionRequest.Amount);

            savingsAccount.Balance = nextBalance;

            await repositoryManager.TransactionRepository.CreateAsync(transaction, cancellationToken).ConfigureAwait(false);

            repositoryManager.SavingsAccountRepository.Update(savingsAccount);
        }

        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        return transaction.Adapt<TransactionResponse>();
    }

    public IEnumerable<TransactionResponse> GetAll()
    {
        var transactions = repositoryManager.TransactionRepository.GetAll();

        return transactions.Adapt<IEnumerable<TransactionResponse>>();
    }

    public async Task<TransactionResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await repositoryManager.TransactionRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new TransactionNotFoundException(id);

        return transaction.Adapt<TransactionResponse>();
    }

    public async Task<AccountTransactionsResponse> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var account = await GetAccount(repositoryManager, accountId, cancellationToken).ConfigureAwait(false);

        var transactions = repositoryManager.TransactionRepository.GetByCondition(transaction => transaction.AccountId == accountId)
            .OrderByDescending(t => t.Date);

        var accountTransactions = new AccountTransactionsResponse
        {
            CheckingAccount = account is CheckingAccount checkingAccount ? checkingAccount.Adapt<CheckingAccountResponse>() : null,
            SavingsAccount = account is SavingsAccount savingsAccount ? savingsAccount.Adapt<SavingsAccountResponse>() : null,
            Transactions = transactions.Adapt<IEnumerable<TransactionResponse>>()
        };

        return accountTransactions;
    }


    public async Task<AccountStatementResponse> GetAccountStatementAsync(AccountStatementQuery accountStatementQuery, CancellationToken cancellationToken = default)
    {
        var account = await GetAccount(repositoryManager, accountStatementQuery.AccountId, cancellationToken).ConfigureAwait(false);

        var endOfSlidingMonth = accountStatementQuery.StartOfSlidingMonth.AddDays(30);

        var transactions = repositoryManager.TransactionRepository.GetByCondition(
            transaction => transaction.AccountId == accountStatementQuery.AccountId
            && transaction.Date >= accountStatementQuery.StartOfSlidingMonth
            && transaction.Date <= endOfSlidingMonth
        ).OrderByDescending(t => t.Date);

        var transactionsResponse = transactions.Adapt<AccountStatementTransactionResponse[]>();

        var accountType = GetAccountType(account);

        return new AccountStatementResponse(accountType, account.Balance, transactionsResponse);
    }

    private static async Task<Domain.Entities.BankAccount> GetAccount(IRepositoryManager repositoryManager, Guid accountId, CancellationToken cancellationToken)
        => await repositoryManager.BankAccountRepository.GetByIdAsync(accountId, cancellationToken).ConfigureAwait(false) ?? throw new TransactionAccountNotFoundException(accountId);

    private readonly Action<float> CheckTransactionAmount = transactionAmount =>
    {
        if (transactionAmount <= 0) throw new InvalidTransactionAmountException();
    };

    private readonly Func<Domain.Entities.BankAccount, string> GetAccountType = account => account is CheckingAccount ? STR_CHECKINGACCOUNT : STR_SAVINGSACCOUNT;
}
