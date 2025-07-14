namespace Domain.Entities;

public class AccountBehaviorData
{
    public string Type { get; set; } = default!;
    public float? OverdraftLimit { get; set; }
    public float? BalanceCeiling { get; set; }
}
