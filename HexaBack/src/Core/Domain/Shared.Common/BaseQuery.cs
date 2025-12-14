#nullable enable
using System.Linq.Expressions;

namespace Domain.Shared.Common;

public abstract record BaseQuery<T>
{
    private Expression<Func<T, bool>>? _filterExpression;

    private const int MaxPageSize = 50;
    private int? _pageSize;
    private int? _page = 1;

    public int? Page
    {
        get => _page ?? 1;
        private set => _page = (value is > 0) ? value : 1;
    }

    public string? OrderBy { get; private set; }
    public string? SearchTerm { get; private set; }

    public int? PageSize
    {
        get => _pageSize ?? 10;
        private set => _pageSize = Math.Min(value ?? 10, MaxPageSize);
    }

    // protected BaseQuery() { }

    public BaseQuery(string? searchTerm, string? orderBy, int? page, int? pageSize)
    {
        SearchTerm = searchTerm;
        OrderBy = orderBy;
        Page = page;
        PageSize = pageSize;
    }

    protected void SetFilterExpression(Expression<Func<T, bool>> filterExpression) => _filterExpression = filterExpression;
    public Expression<Func<T, bool>>? GetFilterExpression() => _filterExpression;
}
