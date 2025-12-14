using System.Collections.Immutable;
using Domain.Abstractions.Events;
using Domain.Entities;
using MCS.WatchTower.WebApi.DataTransferObjects.Enumerations;

namespace Application.Models.Extensions;

public static class DomainEventExtensions
{
    public static BroadcastMessageType GetBroadcastMessageType<T>(this IDomainEvent<T> domainEvent)
        where T : IBaseEntity
    {
        return domainEvent switch
        {
            CreatedEvent<T> => BroadcastMessageType.Created,
            UpdatedEvent<T> => BroadcastMessageType.Updated,
            DeletedEvent<T> => BroadcastMessageType.Deleted,
            _ => throw new ArgumentException($"Unknown event type: {domainEvent.GetType()}")
        };
    }
}
