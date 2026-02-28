namespace Application.DataTransfertObjects.Paging;

public record QueryParameters(string? SearchTerm, string? OrderBy, int? Page, int? PageSize);
