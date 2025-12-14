using System.Collections.Concurrent;
using System.Reflection;
using Domain.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Persistence.Events;

public sealed class EventsDispatcher(IServiceScopeFactory serviceScopeFactory, ILogger<EventsDispatcher> logger) : IEventsDispatcher
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    private readonly ILogger<EventsDispatcher> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    // Optimized cache for event information
    private static readonly ConcurrentDictionary<Type, EventTypeInfo> EventInfoCache = new();

    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(domainEvents);

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        var eventArray = domainEvents switch
        {
            IDomainEvent[] array => array,
            ICollection<IDomainEvent> collection => collection.ToArray(),
            _ => domainEvents.ToArray()
        };

        if (eventArray.Length == 0) return;

        _logger.LogDebug("Dispatching {EventCount} domain events", eventArray.Length);

        using var scope = _serviceScopeFactory.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var eventGroups = GroupEventsByType(eventArray);

        // Optimization: avoid intermediate array allocation
        var tasks = eventGroups.Select(kvp =>
            DispatchEventGroup(kvp.Key, kvp.Value, serviceProvider, cancellationToken));

        await Task.WhenAll(tasks);

        stopwatch.Stop();

        _logger.LogDebug("Dispatched {EventCount} events in {ElapsedMs}ms", eventArray.Length, stopwatch.ElapsedMilliseconds);
    }

    private static Dictionary<Type, List<IDomainEvent>> GroupEventsByType(ReadOnlySpan<IDomainEvent> events)
    {
        var groups = new Dictionary<Type, List<IDomainEvent>>();

        foreach (var evt in events)
        {
            var eventType = evt.GetType();

            if (!groups.TryGetValue(eventType, out var eventList))
            {
                eventList = new List<IDomainEvent>();
                groups[eventType] = eventList;
            }

            eventList.Add(evt);
        }

        return groups;
    }

    private async Task DispatchEventGroup(Type eventType, List<IDomainEvent> events, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var eventInfo = GetOrCreateEventInfo(eventType);
        var handlers = eventInfo.HandlersFactory(serviceProvider);

        if (handlers.Length == 0)
        {
            _logger.LogWarning("No handlers found for event type {EventType}", eventType.Name);
            return;
        }

        _logger.LogDebug("Processing {EventCount} events of type {EventType} with {HandlerCount} handlers",
            events.Count, eventType.Name, handlers.Length);

        var totalTasks = events.Count * handlers.Length;
        var tasks = new Task[totalTasks];
        var taskIndex = 0;

        for (var eventIndex = 0; eventIndex < events.Count; eventIndex++)
        {
            var eventItem = events[eventIndex];
            for (var handlerIndex = 0; handlerIndex < handlers.Length; handlerIndex++)
            {
                var handler = handlers[handlerIndex];
                tasks[taskIndex++] = ExecuteHandlerSafely(handler, eventItem, eventInfo.HandleMethod, cancellationToken);
            }
        }

        await Task.WhenAll(tasks);
    }

    private static EventTypeInfo GetOrCreateEventInfo(Type eventType)
    {
        return EventInfoCache.GetOrAdd(eventType, static et =>
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(et);

            var handleMethod = handlerType.GetMethod("Handle", [et, typeof(CancellationToken)])
                ?? throw new InvalidOperationException($"Handle method not found on {handlerType.Name}");

            var handlersFactory = CreateHandlersFactory(handlerType);

            return new EventTypeInfo
            {
                HandlerType = handlerType,
                HandleMethod = handleMethod,
                HandlersFactory = handlersFactory
            };
        });
    }

    private static Func<IServiceProvider, object[]> CreateHandlersFactory(Type handlerType)
    {
        return serviceProvider =>
        {
            var handlers = serviceProvider.GetServices(handlerType);

            // Consistent filtering in all cases
            return handlers.Where(handler => handler is not null).ToArray();
        };
    }

    private async Task ExecuteHandlerSafely(object handler, IDomainEvent domainEvent, MethodInfo handleMethod, CancellationToken cancellationToken)
    {
        try
        {
            var result = handleMethod.Invoke(handler, [domainEvent, cancellationToken]);

            switch (result)
            {
                case Task task:
                    await task;
                    break;
                case ValueTask valueTask:
                    await valueTask;
                    break;
                case null:
                    throw new InvalidOperationException("Handler method returned null");
                default:
                    throw new InvalidOperationException($"Handler method must return Task or ValueTask, got {result.GetType().Name}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Handler {HandlerType} failed for event {EventType}", handler.GetType().Name, domainEvent.GetType().Name);
        }
    }
}
