using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Domain.Abstractions.Events;
using Domain.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Extensions;

public static class PagingExtensions
{
    public static async Task<PagedList<T>> ApplyPaging<T>(this IQueryable<T> source, int page, int pageSize, CancellationToken cancellationToken) where T : IBaseEntity
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        int previousPage = (page - 1) * pageSize;
        var totalCount = await source.CountAsync(cancellationToken);
        var pagedList = await source.Skip(previousPage).Take(pageSize).ToListAsync(cancellationToken);

        return new PagedList<T>(pagedList, totalCount, page, pageSize);
    }


    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy)) return query;

        IOrderedQueryable<T>? orderedQuery = null;

        foreach (var param in orderBy.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = param.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var propertyName = parts[0];

            var key = $"{typeof(T).FullName}.{propertyName}";

            var lambda = _cache.GetOrAdd(key, _ =>
            {
                var parameter = Expression.Parameter(typeof(T), "x");

                var property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new ArgumentException($"Property '{propertyName}' not found");
                var propertyAccess = Expression.Property(parameter, property);

                return Expression.Lambda(propertyAccess, parameter);
            });

            var descending = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

            orderedQuery = ApplyOrder(query, orderedQuery, lambda, descending);
            query = orderedQuery;
        }

        return query;
    }

    private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, IOrderedQueryable<T>? orderedQuery, LambdaExpression keySelector, bool descending)
    {
        if (orderedQuery == null)
        {
            return descending
                ? Queryable.OrderByDescending(source, (dynamic)keySelector)
                : Queryable.OrderBy(source, (dynamic)keySelector);
        }

        return descending
            ? Queryable.ThenByDescending(orderedQuery, (dynamic)keySelector)
            : Queryable.ThenBy(orderedQuery, (dynamic)keySelector);
    }

    private static readonly ConcurrentDictionary<string, LambdaExpression> _cache = new();
}