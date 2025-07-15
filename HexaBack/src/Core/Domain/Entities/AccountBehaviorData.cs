namespace Domain.Entities;

public class AccountBehaviorData
{
    public string Type { get; set; } = default!;
    public float? OverdraftLimit { get; set; }
    public float? BalanceCeiling { get; set; }

    public static AccountBehaviorData FromDomain(IAccountBehavior behavior) => behavior switch
    {
        CheckingBehavior checking => new AccountBehaviorData
        {
            Type = "Checking",
            OverdraftLimit = checking.OverdraftLimit
        },
        SavingsBehavior savings => new AccountBehaviorData
        {
            Type = "Savings",
            BalanceCeiling = savings.BalanceCeiling
        },
        _ => throw new ArgumentException("Unknown behavior type.")
    };

    public IAccountBehavior ToDomain() => Type switch
    {
        "Checking" => new CheckingBehavior(OverdraftLimit ?? 0),
        "Savings" => new SavingsBehavior(BalanceCeiling ?? 0),
        _ => throw new ArgumentException("Invalid account behavior type.")
    };
}
