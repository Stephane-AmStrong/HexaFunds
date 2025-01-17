using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DataTransfertObjects;
using FluentValidation;

namespace Validators;

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
