#nullable enable

namespace Domain.Entities;

public record Alert : BaseEntity
{
    public required string ServerId { get; set; }
    public required string Type { get; set; }
    public required string Message { get; set; }
    public required string Severity { get; set; }
    public DateTime OccurredAt { get; set; }

    public int Occurrence { get; set; }

    public required string Status { get; set; }

    public DateTime? ActionPerformedAt { get; set; }
    public string? ActionPerformedBy { get; set; }
    public string? ActionComment { get; set; }
    public string? AssignedTo { get; set; }
    public string? AssignmentHint { get; set; }
}
