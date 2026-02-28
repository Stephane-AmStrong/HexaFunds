namespace Application.DataTransfertObjects.QueryParameters;

public record SavingsAccountQueryParameters(
    string? AccountNumber = null,
    float? BalanceLessThan = null,
    float? BalanceGreaterThan = null,
    float? BalanceCeilingLessThan = null,
    float? BalanceCeilingGreaterThan = null,
    string? SearchTerm = null,
    string? OrderBy = null,
    int? Page = null,
    int? PageSize = null
) : Paging.QueryParameters(SearchTerm, OrderBy, Page, PageSize);

