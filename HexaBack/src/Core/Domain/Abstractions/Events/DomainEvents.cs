using Domain.Entities;

namespace Domain.Abstractions.Events;

public record CreatedEvent<T>(T Record) : IDomainEvent<T> where T : IBaseEntity;
public record UpdatedEvent<T>(T Record) : IDomainEvent<T> where T : IBaseEntity;
public record DeletedEvent<T>(T Record) : IDomainEvent<T> where T : IBaseEntity;
