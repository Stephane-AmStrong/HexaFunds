#nullable enable
using System.Linq.Expressions;
using System.Reflection;
using Domain.Abstractions.Events;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Shared.Common;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Persistence.Repository;

public class RepositoryBase<T> : IRepositoryBase<T> where T : IBaseEntity
{
    protected IMongoCollection<T> Collection { get; }
    private readonly IEventsDispatcher _eventsDispatcher;

    protected RepositoryBase(IMongoDatabase database, IEventsDispatcher eventsDispatcher, string collectionName)
    {
        _eventsDispatcher = eventsDispatcher;
        Collection = database.GetCollection<T>(collectionName);
    }


    public async Task<PagedList<T>> BaseQueryWithFiltersAsync(BaseQuery<T> queryParameters, CancellationToken cancellationToken)
    {
        List<FilterDefinition<T>> filters = [];

        // Add expression filter if any
        var filterExpression = queryParameters.GetFilterExpression();
        if (filterExpression != null) filters.Add(Builders<T>.Filter.Where(filterExpression));

        // Add text search filter if any
        if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
        {
            var stringProps = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(string));

            var regexFilters = stringProps
                .Select(p => Builders<T>.Filter.Regex(p.Name, new BsonRegularExpression(queryParameters.SearchTerm, "i")));

            IEnumerable<FilterDefinition<T>> filterDefinitions = regexFilters as FilterDefinition<T>[] ?? regexFilters.ToArray();
            if (filterDefinitions.Any())
            {
                var textSearchFilter = Builders<T>.Filter.Or(filterDefinitions);
                filters.Add(textSearchFilter);
            }
        }

        // Combine all filters with AND if more than one, otherwise Empty
        var filter = filters.Count switch
        {
            0 => Builders<T>.Filter.Empty,
            1 => filters[0],
            _ => Builders<T>.Filter.And(filters)
        };

        var find = Collection.Find(filter);

        var totalCount = await Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var sort = BuildSortDefinition(queryParameters.OrderBy);

        if (sort != null) find = find.Sort(sort);

        find = find.Skip((queryParameters.Page - 1) * queryParameters.PageSize).Limit(queryParameters.PageSize);

        return new PagedList<T>(await find.ToListAsync(cancellationToken), totalCount, queryParameters.Page!.Value, queryParameters.PageSize!.Value);
    }

    protected async Task<List<T>> BaseGetLatestByGroupAsync<TKey, TOrder>(
        Expression<Func<T, bool>> filterPredicate,
        Expression<Func<T, TKey>> groupBySelector,
        Expression<Func<T, TOrder>> orderBySelector,
        CancellationToken cancellationToken)
    {
        var filter = Builders<T>.Filter.Where(filterPredicate);

        var groupByProperty = GetPropertyName(groupBySelector);
        var orderByProperty = GetPropertyName(orderBySelector);

        var pipeline = new BsonDocument[]
        {
            new BsonDocument("$match", filter.Render(Collection.DocumentSerializer, Collection.Settings.SerializerRegistry)),

            new BsonDocument("$sort", new BsonDocument
            {
                [groupByProperty] = 1,
                [orderByProperty] = -1
            }),

            new BsonDocument("$group", new BsonDocument
            {
                ["_id"] = $"${groupByProperty}",
                ["latestDocument"] = new BsonDocument("$first", "$$ROOT")
            }),

            new BsonDocument("$replaceRoot", new BsonDocument
            {
                ["newRoot"] = "$latestDocument"
            })
        };

        var cursor = await Collection.AggregateAsync<T>(pipeline, cancellationToken: cancellationToken);
        return await cursor.ToListAsync(cancellationToken);
    }

    public Task<List<T>> BaseFindByConditionAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
    {
        return Collection.Find(expression).ToListAsync(cancellationToken);
    }

    public async Task BaseCreateAsync(T entity, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(entity.Id))
        {
            string? newId = ObjectId.GenerateNewId().ToString();
            entity.Id = newId;
        }

        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        await DispatchDomainEventsAsync(entity, cancellationToken);
    }

    public async Task BaseUpdateAsync(T entity, CancellationToken cancellationToken)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id);
        await Collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
        await DispatchDomainEventsAsync(entity, cancellationToken);
    }

    public Task BaseDeleteAsync(T entity, CancellationToken cancellationToken)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id);

        var deleteTask = Collection.DeleteOneAsync(filter, cancellationToken);

        _ = DispatchDomainEventsAsync(entity, cancellationToken);

        return deleteTask;
    }

    public async Task<long> BaseBulkOperationsAsync(ISet<T> entities, BulkOperation operation, CancellationToken cancellationToken)
    {
        var writeModels = new WriteModel<T>[entities.Count];
        var index = 0;

        foreach (var entity in entities)
        {
            if (operation == BulkOperation.Insert && string.IsNullOrWhiteSpace(entity.Id))
            {
                entity.Id = ObjectId.GenerateNewId().ToString();
            }

            writeModels[index++] = operation switch
            {
                BulkOperation.Insert => new InsertOneModel<T>(entity),
                BulkOperation.Update => new ReplaceOneModel<T>(Builders<T>.Filter.Eq(e => e.Id, entity.Id), entity),
                BulkOperation.Delete => new DeleteOneModel<T>(Builders<T>.Filter.Eq(e => e.Id, entity.Id)),
                _ => throw new ArgumentException($"Unsupported operation: {operation}")
            };
        }

        var result = await Collection.BulkWriteAsync(writeModels, new BulkWriteOptions { IsOrdered = false }, cancellationToken);

        var entitiesWithEvents = entities.Where(e => e.DomainEvents.Count > 0);
        foreach (var entity in entitiesWithEvents)
        {
            await DispatchDomainEventsAsync(entity, cancellationToken);
        }

        return result.InsertedCount + result.ModifiedCount + result.DeletedCount;
    }

    private SortDefinition<T>? BuildSortDefinition(string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return null;

        var orderParams = orderByQueryString.Trim().Split(',');
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var sortDefinitions = new List<SortDefinition<T>>();

        foreach (var param in orderParams)
        {
            var trimmedParam = param?.Trim();
            if (string.IsNullOrWhiteSpace(trimmedParam)) continue;

            var parts = trimmedParam.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            var propertyFromQueryName = parts[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

            if (objectProperty == null) continue;

            var isDescending = trimmedParam.EndsWith(" desc", StringComparison.InvariantCultureIgnoreCase);
            var sortDef = isDescending
                ? Builders<T>.Sort.Descending(objectProperty.Name)
                : Builders<T>.Sort.Ascending(objectProperty.Name);

            sortDefinitions.Add(sortDef);
        }

        return sortDefinitions.Count > 0 ? Builders<T>.Sort.Combine(sortDefinitions) : null;
    }

    private static string GetPropertyName<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyExpression)
    {
        if (propertyExpression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        throw new ArgumentException("Expression must be a property access", nameof(propertyExpression));
    }

    private async Task DispatchDomainEventsAsync(T entity, CancellationToken cancellationToken)
    {
        if (entity.DomainEvents.Count != 0)
        {
            await _eventsDispatcher.DispatchAsync(entity.DomainEvents, cancellationToken);
            entity.DomainEvents.Clear();
        }
    }
}
