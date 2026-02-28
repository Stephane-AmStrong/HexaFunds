namespace Application.DataTransfertObjects.QueryParameters;

public record TransactionQueryParameters(
    Guid? AccountId = null,
    string? AccountNumber = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    string? SearchTerm = null,
    string? OrderBy = null,
    int? Page = null,
    int? PageSize = null
) : Paging.QueryParameters(SearchTerm, OrderBy, Page, PageSize);