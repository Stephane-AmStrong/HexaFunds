using Application.DataTransfertObjects;
using FluentValidation;

namespace WebApi.Validators;

public class BankAccountValidator : AbstractValidator<BankAccountRequest>
{
    public BankAccountValidator()
    {
        RuleFor(bankAccount => bankAccount.AccountNumber)
            .NotEmpty();
    }
}
