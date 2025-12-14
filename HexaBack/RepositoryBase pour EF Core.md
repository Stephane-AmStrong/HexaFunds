Voici une version **Entity Framework Core** équivalente, avec **typage fort** et **sans réflexion** :

---

## 1️⃣ **RepositoryBase<T> pour EF Core**

```csharp
#nullable enable
using System.Linq.Expressions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, IBaseEntity
{
    protected DbContext Context { get; }
    protected DbSet<T> DbSet { get; }

    protected RepositoryBase(DbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public async Task<PagedList<T>> BaseQueryWithFiltersAsync(QueryParameters<T> queryParameters, CancellationToken cancellationToken)
    {
        var query = DbSet.AsQueryable();

        // Add expression filter if any
        var filterExpression = queryParameters.GetFilterExpression();
        if (filterExpression != null)
        {
            query = query.Where(filterExpression);
        }

        // Add text search filter if any (delegate to concrete implementation)
        if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
        {
            query = ApplyTextSearch(query, queryParameters.SearchTerm);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply sorting
        query = ApplyOrdering(query, queryParameters.OrderBy);

        // Apply pagination
        query = query
            .Skip((queryParameters.Page - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize);

        var items = await query.ToListAsync(cancellationToken);

        return new PagedList<T>(items, totalCount, queryParameters.Page!.Value, queryParameters.PageSize!.Value);
    }

    public Task<List<T>> BaseFindByConditionAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
    {
        return DbSet.Where(expression).ToListAsync(cancellationToken);
    }

    public async Task BaseCreateAsync(T entity, CancellationToken cancellationToken)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task BaseUpdateAsync(T entity, CancellationToken cancellationToken)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task BaseDeleteAsync(string id, CancellationToken cancellationToken)
    {
        var entity = await DbSet.FindAsync([id], cancellationToken);
        if (entity != null)
        {
            DbSet.Remove(entity);
            await Context.SaveChangesAsync(cancellationToken);
        }
    }

    // Abstract methods for type-safe implementations
    protected abstract IQueryable<T> ApplyTextSearch(IQueryable<T> query, string searchTerm);
    protected abstract IQueryable<T> ApplyOrdering(IQueryable<T> query, string? orderBy);
}
```

---

## 2️⃣ **Implémentation concrète pour Alert**

```csharp
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository;

public class AlertsRepository : RepositoryBase<Alert>, IAlertsRepository
{
    public AlertsRepository(DbContext context) : base(context) { }

    // Type-safe text search implementation
    protected override IQueryable<Alert> ApplyTextSearch(IQueryable<Alert> query, string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        return query.Where(alert =>
            (alert.Message != null && alert.Message.ToLower().Contains(lowerSearchTerm)) ||
            (alert.Type != null && alert.Type.ToLower().Contains(lowerSearchTerm)) ||
            (alert.ServerId != null && alert.ServerId.ToLower().Contains(lowerSearchTerm))
        );
    }

    // Type-safe ordering implementation
    protected override IQueryable<Alert> ApplyOrdering(IQueryable<Alert> query, string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return query.OrderByDescending(a => a.OccurredAt); // Default sort

        return orderBy.ToLower() switch
        {
            "occurredat" => query.OrderBy(a => a.OccurredAt),
            "occurredat desc" => query.OrderByDescending(a => a.OccurredAt),
            "severity" => query.OrderBy(a => a.Severity),
            "severity desc" => query.OrderByDescending(a => a.Severity),
            "type" => query.OrderBy(a => a.Type),
            "type desc" => query.OrderByDescending(a => a.Type),
            "message" => query.OrderBy(a => a.Message),
            "message desc" => query.OrderByDescending(a => a.Message),
            _ => query.OrderByDescending(a => a.OccurredAt) // Default fallback
        };
    }

    // Delegate to base methods for interface implementation
    public Task<PagedList<Alert>> GetPagedListByQueryAsync(AlertQuery queryParameters, CancellationToken cancellationToken)
        => BaseQueryWithFiltersAsync(queryParameters, cancellationToken);

    public Task<List<Alert>> FindByConditionAsync(Expression<Func<Alert, bool>> expression, CancellationToken cancellationToken)
        => BaseFindByConditionAsync(expression, cancellationToken);

    public Task<Alert?> GetByIdAsync(string id, CancellationToken cancellationToken)
        => DbSet.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public Task CreateAsync(Alert alert, CancellationToken cancellationToken)
        => BaseCreateAsync(alert, cancellationToken);

    public Task UpdateAsync(Alert alert, CancellationToken cancellationToken)
        => BaseUpdateAsync(alert, cancellationToken);

    public Task DeleteAsync(string id, CancellationToken cancellationToken)
        => BaseDeleteAsync(id, cancellationToken);
}
```

---

## 3️⃣ **Alternative : Builder pattern pour le tri**

Si tu veux un tri encore plus type-safe, tu peux utiliser un **builder pattern** :

```csharp
public static class QueryExtensions
{
    public static IQueryable<Alert> ApplyAlertOrdering(this IQueryable<Alert> query, string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return query.OrderByDescending(a => a.OccurredAt);

        var orderBuilder = new OrderBuilder<Alert>(query);

        var orderParams = orderBy.Split(',', StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var param in orderParams)
        {
            var trimmed = param.Trim();
            var isDescending = trimmed.EndsWith(" desc", StringComparison.OrdinalIgnoreCase);
            var property = isDescending ? trimmed[..^5].Trim() : trimmed;

            orderBuilder = property.ToLower() switch
            {
                "occurredat" => isDescending ? orderBuilder.ThenByDescending(a => a.OccurredAt) : orderBuilder.ThenBy(a => a.OccurredAt),
                "severity" => isDescending ? orderBuilder.ThenByDescending(a => a.Severity) : orderBuilder.ThenBy(a => a.Severity),
                "type" => isDescending ? orderBuilder.ThenByDescending(a => a.Type) : orderBuilder.ThenBy(a => a.Type),
                "message" => isDescending ? orderBuilder.ThenByDescending(a => a.Message) : orderBuilder.ThenBy(a => a.Message),
                _ => orderBuilder
            };
        }

        return orderBuilder.Build() ?? query.OrderByDescending(a => a.OccurredAt);
    }
}

public class OrderBuilder<T>
{
    private IOrderedQueryable<T>? _query;
    private readonly IQueryable<T> _baseQuery;

    public OrderBuilder(IQueryable<T> query) => _baseQuery = query;

    public OrderBuilder<T> ThenBy<TKey>(Expression<Func<T, TKey>> keySelector)
    {
        _query = _query?.ThenBy(keySelector) ?? _baseQuery.OrderBy(keySelector);
        return this;
    }

    public OrderBuilder<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
    {
        _query = _query?.ThenByDescending(keySelector) ?? _baseQuery.OrderByDescending(keySelector);
        return this;
    }

    public IOrderedQueryable<T>? Build() => _query;
}
```

---

## 🎯 **Avantages de cette approche**

✅ **100% type-safe** : Aucune réflexion, tout en expressions LINQ  
✅ **Performance optimale** : EF Core génère du SQL optimal  
✅ **Extensible** : Chaque repository peut définir sa logique de recherche/tri  
✅ **Maintenable** : Les erreurs de compilation détectent les incohérences  
✅ **Flexible** : Support des tris multi-colonnes via le builder pattern

Cette approche combine la **réutilisabilité** du pattern générique avec la **sécurité de type** d'Entity Framework Core !