namespace Domain.Abstractions.Events;

public interface IBaseEntity
{
    public Guid Id { get; set; }
    void ClearDomainEvents();
    List<IDomainEvent> DomainEvents { get; }
    void Raise(IDomainEvent domainEvent);
}