using System.Linq.Expressions;

namespace Domain.Shared.Common;

public abstract record BaseQuery<T>
{
    private Expression<Func<T, bool>>? _filterExpression;

    private const int DefaultPageSize = 10;
    private const int DefaultPage = 1;
    private const int MaxPageSize = 50;

    private int? _pageSize;
    private int? _page = DefaultPage;

    public int Page
    {
        get => _page ?? DefaultPage;
        private set => _page = (value is > 0) ? value : DefaultPage;
    }

    public string? OrderBy { get; private set; }
    public string? SearchTerm { get; private set; }

    public int PageSize
    {
        get => _pageSize ?? DefaultPageSize;
        private set => _pageSize = value > 0 ? Math.Min(value, MaxPageSize) : DefaultPageSize;
    }

    public BaseQuery(string? searchTerm, string? orderBy, int? page, int? pageSize)
    {
        SearchTerm = searchTerm;
        OrderBy = orderBy;
        Page = page ?? DefaultPage;
        PageSize = pageSize ?? DefaultPageSize;
    }

    protected void SetFilterExpression(Expression<Func<T, bool>> filterExpression) => _filterExpression = filterExpression;
    public Expression<Func<T, bool>>? GetFilterExpression() => _filterExpression;
}
