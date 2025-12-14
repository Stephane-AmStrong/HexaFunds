#nullable enable
using Domain.Entities;
using Domain.Shared.Common;

namespace Application.UseCases.CheckingAccounts.GetByQuery;

public record CheckingAccountQuery : QueryParameters<CheckingAccount>
{
    public string? WithAccountNumber { get; init; }
    public float? WithOverdraftLimit { get; init; }
    public float? WithBalanceGreaterThan { get; init; }
    public float? WithBalanceLessThan { get; init; }

    public CheckingAccountQuery(string? withAccountNumber, float? withOverdraftLimit, float? withBalanceGreaterThan, float? withBalanceLessThan, string? searchTerm, string? orderBy, int? page, int? pageSize) : base(searchTerm, orderBy, page, pageSize)
    {
        WithAccountNumber = withAccountNumber;
        WithOverdraftLimit = withOverdraftLimit;
        WithBalanceGreaterThan = withBalanceGreaterThan;
        WithBalanceLessThan = withBalanceLessThan;

        if (!string.IsNullOrWhiteSpace(withAccountNumber) || withOverdraftLimit is not null || withBalanceGreaterThan is not null || withBalanceLessThan is not null)
        {
            SetFilterExpression
            (
                checkingaccount => (string.IsNullOrWhiteSpace(withAccountNumber) || checkingaccount.AccountNumber == withAccountNumber) &&
                        (withOverdraftLimit == null || checkingaccount.OverdraftLimit == withOverdraftLimit) &&
                        (withBalanceGreaterThan == null || checkingaccount.Balance < withBalanceGreaterThan) &&
                        (withBalanceLessThan == null || checkingaccount.Balance >= withBalanceLessThan)
            );
        }
    }
}