using Application.DataTransfertObjects;
using FluentValidation;

namespace WebApi.Validators;

public class SavingsAccountValidator : AbstractValidator<SavingsAccountRequest>
{
    public SavingsAccountValidator()
    {
        RuleFor(savingsAccount => savingsAccount.AccountNumber)
            .NotEmpty();

        RuleFor(savingsAccount => savingsAccount.BalanceCeiling)
            .GreaterThan(0)
            .LessThan(float.MaxValue);
    }
}
