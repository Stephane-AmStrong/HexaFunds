namespace Domain.Entities;

public record Connection : BaseEntity
{
    public string ClientId { get; init; }
    public string ServerId { get; init; }
    public string ProcessId { get; init; }
    public string ApiVersion { get; init; }
    public string Machine { get; init; }
    public string Application { get; init; }
}
