using Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Configurations;

public class AccountBehaviorConverter2 : ValueConverter<IAccountBehavior, string>
{
    public AccountBehaviorConverter2() : base(behavior => AccountBehaviorToString(behavior), value => StringToAccountBehavior(value))
    {
    }

    private static string AccountBehaviorToString(IAccountBehavior accountBehavior) => accountBehavior switch
    {
        CheckingBehavior checking => $"Checking {checking.OverdraftLimit}",
        SavingsBehavior savings => $"Savings {savings.BalanceCeiling}",
        _ => throw new ArgumentException($"Unsupported IAccountBehavior type: {accountBehavior.GetType().Name}")
    };

    private static IAccountBehavior StringToAccountBehavior(string kind)
    {
        var parts = kind.Split(' ');

        return parts[0] switch
        {
            "Checking" when float.TryParse(parts[1], out var overdraftLimit) => new CheckingBehavior(overdraftLimit),

            "Savings" when float.TryParse(parts[1], out var balanceCeiling) => new SavingsBehavior(balanceCeiling),

            _ => throw new ArgumentException($"Invalid AccountBehavior string: {kind}")
        };
    }
}
