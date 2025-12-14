Voici la version **Entity Framework Core** avec tes préférences de style et des noms anonymisés :

```csharp
#nullable enable
using System.Linq.Expressions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository;

public abstract class EntityRepositoryBase<T> : IEntityRepositoryBase<T> where T : class
{
    protected WatchTowerDbContext DbContext { get; set; }

    public EntityRepositoryBase(WatchTowerDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public IQueryable<T> BaseFindAll()
    {
        return DbContext.Set<T>().AsNoTracking();
    }

    public IQueryable<T> BaseFindByCondition(Expression<Func<T, bool>> expression)
    {
        return DbContext.Set<T>().Where(expression).AsNoTracking();
    }

    public async Task<PagedList<T>> BaseGetPagedAsync(QueryParameters<T> queryParams, CancellationToken cancellationToken)
    {
        var query = DbContext.Set<T>().AsQueryable();

        // Apply filter expression if any
        var filterExpr = queryParams.GetFilterExpression();
        if (filterExpr != null)
        {
            query = query.Where(filterExpr);
        }

        // Apply text search (delegate to concrete implementation)
        if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
        {
            query = ApplyTextFilter(query, queryParams.SearchTerm);
        }

        // Get total count before pagination
        var totalItems = await query.CountAsync(cancellationToken);

        // Apply sorting
        query = ApplySorting(query, queryParams.OrderBy);

        // Apply pagination
        query = query
            .Skip((queryParams.Page - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize);

        var items = await query.ToListAsync(cancellationToken);

        return new PagedList<T>(items, totalItems, queryParams.Page!.Value, queryParams.PageSize!.Value);
    }

    public async Task BaseCreateAsync(T entity)
    {
        await DbContext.Set<T>().AddAsync(entity);
    }

    public async Task BaseCreateAsync(IEnumerable<T> entities)
    {
        await DbContext.Set<T>().AddRangeAsync(entities);
    }

    public async Task BaseUpdateAsync(T entity)
    {
        await Task.Run(() => DbContext.Set<T>().Update(entity));
    }

    public async Task BaseUpdateAsync(IEnumerable<T> entities)
    {
        await Task.Run(() => DbContext.Set<T>().UpdateRange(entities));
    }

    public async Task BaseDeleteAsync(T entity)
    {
        await Task.Run(() => DbContext.Set<T>().Remove(entity));
    }

    public async Task BaseSaveChangesAsync()
    {
        await DbContext.SaveChangesAsync();
    }

    // Abstract methods for type-safe implementations
    protected abstract IQueryable<T> ApplyTextFilter(IQueryable<T> query, string searchTerm);
    protected abstract IQueryable<T> ApplySorting(IQueryable<T> query, string? orderBy);
}
```

---

## **Implémentation concrète pour Alert**

```csharp
using Domain.Entities;

namespace Persistence.Repository;

public class AlertEntityRepository : EntityRepositoryBase<Alert>, IAlertEntityRepository
{
    public AlertEntityRepository(WatchTowerDbContext dbContext) : base(dbContext) { }

    // Type-safe text search implementation
    protected override IQueryable<Alert> ApplyTextFilter(IQueryable<Alert> query, string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        return query.Where(alert =>
            (alert.Message != null && alert.Message.ToLower().Contains(lowerSearchTerm)) ||
            (alert.Type != null && alert.Type.ToLower().Contains(lowerSearchTerm)) ||
            (alert.ServerId != null && alert.ServerId.ToLower().Contains(lowerSearchTerm))
        );
    }

    // Type-safe ordering implementation
    protected override IQueryable<Alert> ApplySorting(IQueryable<Alert> query, string? orderBy)
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

    // Interface implementation
    public Task<PagedList<Alert>> GetPagedListByQueryAsync(AlertQuery queryParams, CancellationToken cancellationToken)
        => BaseGetPagedAsync(queryParams, cancellationToken);

    public Task<List<Alert>> FindByConditionAsync(Expression<Func<Alert, bool>> expression, CancellationToken cancellationToken)
        => BaseFindByCondition(expression).ToListAsync(cancellationToken);

    public Task<Alert?> GetByIdAsync(string id, CancellationToken cancellationToken)
        => DbContext.Set<Alert>().FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task CreateAsync(Alert alert, CancellationToken cancellationToken)
    {
        await BaseCreateAsync(alert);
        await BaseSaveChangesAsync();
    }

    public async Task UpdateAsync(Alert alert, CancellationToken cancellationToken)
    {
        await BaseUpdateAsync(alert);
        await BaseSaveChangesAsync();
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Set<Alert>().FindAsync([id], cancellationToken);
        if (entity != null)
        {
            await BaseDeleteAsync(entity);
            await BaseSaveChangesAsync();
        }
    }
}
```

---

## **Changements apportés :**

- `ApplicationDbContext` → `WatchTowerDbContext`
- `RepositoryBase` → `EntityRepositoryBase`
- `BaseQueryWithFiltersAsync` → `BaseGetPagedAsync`
- Variables renommées : `queryParameters` → `queryParams`, `totalCount` → `totalItems`
- Ajout de `BaseSaveChangesAsync()` pour séparer les opérations du commit
- Structure similaire à ton style avec des méthodes abstraites pour le typage fort

Cette approche garde tes préférences de style tout en apportant le **typage fort** et **l'absence de réflexion** !