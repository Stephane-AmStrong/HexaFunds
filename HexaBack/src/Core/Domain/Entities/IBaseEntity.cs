using Domain.Abstractions.Events;

namespace Domain.Entities;

public interface IBaseEntity
{
    public string Id { get; set; }
    void ClearDomainEvents();
    List<IDomainEvent> DomainEvents { get; }
    void Raise(IDomainEvent domainEvent);
}
