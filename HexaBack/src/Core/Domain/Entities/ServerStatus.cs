namespace Domain.Entities;

public record ServerStatus : BaseEntity
{
    public required string ServerId { get; init; }
    public required string Status { get; init; }
    public DateTime RecordedAt { get; init; }
}
