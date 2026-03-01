#nullable enable
using Domain.Entities;
using Domain.Shared.Common;
using Application.DataTransfertObjects.QueryParameters;

namespace Application.UseCases.CheckingAccounts.GetByQuery;

public record CheckingAccountQuery : BaseQuery<CheckingAccount>
{
    public CheckingAccountQuery(CheckingAccountQueryParameters queryParameters) : base(queryParameters.SearchTerm, queryParameters.OrderBy, queryParameters.Page, queryParameters.PageSize)
    {
        if (!string.IsNullOrWhiteSpace(queryParameters.AccountNumber) || queryParameters.BalanceLessThan is not null || queryParameters.BalanceGreaterThan is not null || queryParameters.OverdraftLimitLessThan is not null || queryParameters.OverdraftLimitGreaterThan is not null)
        {
            SetFilterExpression
            (
                checkingAccount => (string.IsNullOrWhiteSpace(queryParameters.AccountNumber) || checkingAccount.AccountNumber == queryParameters.AccountNumber) && (queryParameters.BalanceLessThan == null || checkingAccount.Balance <= queryParameters.BalanceLessThan) && (queryParameters.BalanceGreaterThan == null || checkingAccount.Balance >= queryParameters.BalanceGreaterThan) && (queryParameters.OverdraftLimitLessThan == null || checkingAccount.OverdraftLimit <= queryParameters.OverdraftLimitLessThan) && (queryParameters.OverdraftLimitGreaterThan == null || checkingAccount.OverdraftLimit >= queryParameters.OverdraftLimitGreaterThan)
            );
        }
    }
}