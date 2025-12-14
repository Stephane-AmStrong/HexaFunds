namespace Domain.Entities;

public record Host : BaseEntity
{
    public string Name { get; init; }
}
