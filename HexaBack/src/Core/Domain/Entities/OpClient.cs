namespace Domain.Entities;

public record OpClient : Client
{
    public int OrderExecCount { get; init; }
    public int OrderQueryCount { get; init; }
}
