using System.ComponentModel.DataAnnotations;
using Application.DataTransfertObjects;
using FluentValidation;

namespace WebApi.Validators;

public class CheckingAccountValidator : AbstractValidator<CheckingAccountBehaviorRequest>
{
    public CheckingAccountValidator()
    {
        // RuleFor(chechingAccount => chechingAccount.AccountNumber)
        //     .NotEmpty();

        RuleFor(chechingAccount => chechingAccount.OverdraftLimit)
            .GreaterThan(0)
            .LessThan(float.MaxValue);
    }
}
