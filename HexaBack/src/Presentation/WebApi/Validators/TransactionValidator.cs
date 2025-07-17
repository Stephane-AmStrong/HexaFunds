using Application.DataTransfertObjects;
using FluentValidation;

namespace WebApi.Validators;

public class TransactionValidator : AbstractValidator<TransactionRequest>
{
    public TransactionValidator()
    {
        RuleFor(transaction => transaction.Amount)
            .GreaterThan(0)
            .LessThan(float.MaxValue);

        RuleFor(transaction => transaction.AccountId)
            .NotEmpty();
    }
}
