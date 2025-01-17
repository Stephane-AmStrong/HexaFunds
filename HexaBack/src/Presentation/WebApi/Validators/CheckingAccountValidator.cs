using System.ComponentModel.DataAnnotations;
using DataTransfertObjects;
using FluentValidation;

namespace Validators;

public class CheckingAccountValidator : AbstractValidator<CheckingAccountRequest>
{
    public CheckingAccountValidator()
    {
        RuleFor(chechingAccount => chechingAccount.AccountNumber)
            .NotEmpty();

        RuleFor(chechingAccount => chechingAccount.OverdraftLimit)
            .GreaterThan(0)
            .LessThan(float.MaxValue);
    }
}
