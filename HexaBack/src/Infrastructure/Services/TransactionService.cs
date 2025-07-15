
using Application.DataTransfertObjects;
using Application.Services.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories.Abstractions;

using Mapster;


namespace Services;

public sealed class TransactionService(
    ITransactionRepository transactionRepository,
    IBankAccountRepository bankAccountRepository,
    IUnitOfWork unitOfWork
    ) : ITransactionService
{
    private const string STR_CHECKINGACCOUNT = "Compte Courant";
    private const string STR_SAVINGSACCOUNT = "Livret ";

    public async Task<TransactionResponse> CreateAsync(TransactionRequest transactionRequest, CancellationToken cancellationToken = default)
    {
        var account = await GetAccount(transactionRequest.AccountId, cancellationToken).ConfigureAwait(false);

        var transaction = transactionRequest.Adapt<Transaction>();

        account.ApplyTransaction(transaction);

        await transactionRepository.CreateAsync(transaction, cancellationToken).ConfigureAwait(false);

        bankAccountRepository.Update(account);

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


    private readonly Func<Domain.Entities.BankAccount, string> GetAccountType = account => account is CheckingBehavior ? STR_CHECKINGACCOUNT : STR_SAVINGSACCOUNT;
}
