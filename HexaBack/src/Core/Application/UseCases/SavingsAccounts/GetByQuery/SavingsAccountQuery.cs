#nullable enable
using Domain.Entities;
using Domain.Shared.Common;
using Application.DataTransfertObjects.QueryParameters;

namespace Application.UseCases.SavingsAccounts.GetByQuery;

public record SavingsAccountQuery : BaseQuery<SavingsAccount>
{
    public SavingsAccountQuery(SavingsAccountQueryParameters queryParameters) : base(queryParameters.SearchTerm, queryParameters.OrderBy, queryParameters.Page, queryParameters.PageSize)
    {
        if (!string.IsNullOrWhiteSpace(queryParameters.AccountNumber) || queryParameters.BalanceLessThan is not null || queryParameters.BalanceGreaterThan is not null || queryParameters.BalanceCeilingLessThan is not null || queryParameters.BalanceCeilingGreaterThan is not null)
        {
            SetFilterExpression
            (
                savingsAccount => (string.IsNullOrWhiteSpace(queryParameters.AccountNumber) || savingsAccount.AccountNumber == queryParameters.AccountNumber) && (queryParameters.BalanceLessThan == null || savingsAccount.Balance <= queryParameters.BalanceLessThan) && (queryParameters.BalanceGreaterThan == null || savingsAccount.Balance >= queryParameters.BalanceGreaterThan) && (queryParameters.BalanceCeilingLessThan == null || savingsAccount.BalanceCeiling <= queryParameters.BalanceCeilingLessThan) && (queryParameters.BalanceCeilingGreaterThan == null || savingsAccount.BalanceCeiling >= queryParameters.BalanceCeilingGreaterThan)
            );
        }
    }
}