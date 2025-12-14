namespace Domain.Entities;

public record Server : BaseEntity
{
    public string HostName { get; init; }
    public string AppName { get; init; }
    public string Port { get; init; }

    public string Type { get; init; }
    public string RunMode { get; init; }

    public string CronStartTime { get; init; }
    public string CronStopTime { get; init; }

    public string Version { get; init; }
    public IList<string> Tags { get; init; }
}
