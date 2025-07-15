using Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Configurations;

public class AccountBehaviorConverter : ValueConverter<IAccountBehavior, AccountBehaviorData>
{
    public AccountBehaviorConverter() : base(behavior => AccountBehaviorToAccountBehaviorData(behavior), data => AccountBehaviorDataToAccountBehavior(data))
    {
    }

    private static AccountBehaviorData AccountBehaviorToAccountBehaviorData(IAccountBehavior accountBehavior) => accountBehavior switch
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
        _ => throw new ArgumentException($"Unsupported IAccountBehavior type: {accountBehavior.GetType().Name}")
    };

    private static IAccountBehavior AccountBehaviorDataToAccountBehavior(AccountBehaviorData data) => data.Type switch
    {
        "Checking" => new CheckingBehavior(data.OverdraftLimit ?? 0),
        "Savings" => new SavingsBehavior(data.BalanceCeiling ?? 0),
        _ => throw new ArgumentException($"Unsupported AccountBehavior type: {data.Type}")
    };
}
