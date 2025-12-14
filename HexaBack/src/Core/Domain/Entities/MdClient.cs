namespace Domain.Entities;

public record MdClient : Client
{
    public int InstrumentsCount { get; init; }
    public Dictionary<string, int> SubscriptionByUnderlying { get; init; }
}
