#nullable enable
using Domain.Entities;
using Domain.Shared.Common;
using Application.DataTransfertObjects.QueryParameters;

namespace Application.UseCases.Transactions.GetByQuery;

public record TransactionQuery : BaseQuery<Transaction>
{
    public TransactionQuery(TransactionQueryParameters queryParameters) : base(queryParameters.SearchTerm, queryParameters.OrderBy, queryParameters.Page, queryParameters.PageSize)
    {
        if (queryParameters.AccountId is not null || !string.IsNullOrWhiteSpace(queryParameters.AccountNumber) || queryParameters.FromDate is not null || queryParameters.ToDate is not null)
        {
            SetFilterExpression
            (
                transaction => (string.IsNullOrWhiteSpace(queryParameters.AccountNumber) || transaction.BankAccount.AccountNumber == queryParameters.AccountNumber) && (queryParameters.FromDate == null || transaction.Date <= queryParameters.FromDate) && (queryParameters.ToDate == null || transaction.Date >= queryParameters.ToDate)
            );
        }
    }
}