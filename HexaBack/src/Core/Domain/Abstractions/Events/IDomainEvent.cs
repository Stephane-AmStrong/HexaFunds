using Domain.Entities;

namespace Domain.Abstractions.Events;

public interface IDomainEvent;

public interface IDomainEvent<out T> : IDomainEvent where T : IBaseEntity
{
    T Record { get; }
}
