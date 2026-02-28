namespace Application.DataTransfertObjects.QueryParameters;

public record CheckingAccountQueryParameters(
    string? AccountNumber = null,
    float? BalanceLessThan = null,
    float? BalanceGreaterThan = null,
    float? OverdraftLimitLessThan = null,
    float? OverdraftLimitGreaterThan = null,
    string? SearchTerm = null,
    string? OrderBy = null,
    int? Page = null,
    int? PageSize = null
) : Paging.QueryParameters(SearchTerm, OrderBy, Page, PageSize);
