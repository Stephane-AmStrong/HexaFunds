using Domain.Entities;

namespace Domain.Extensions;

public static class AccountBehaviorExtensions
{
    public static TResult Map<TResult>(this IAccountBehavior accountBehavior, Func<CheckingBehavior, TResult> checking, Func<SavingsBehavior, TResult> savings) => accountBehavior switch
    {
        CheckingBehavior checkccountBehavior => checking(checkccountBehavior),
        SavingsBehavior savingsBehavior => savings(savingsBehavior),
        _ => throw new ArgumentException($"Unknown account behavior : {accountBehavior}")
    };
}
