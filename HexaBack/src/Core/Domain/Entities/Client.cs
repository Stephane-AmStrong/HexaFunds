namespace Domain.Entities;

public record Client : BaseEntity
{
    public string Gaia { get; init; }
    public string Login { get; init; }
}
