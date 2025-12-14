using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.Alerts.Delete;

public class DeleteAlertValidator : AbstractValidator<DeleteAlertCommand>
{
    public DeleteAlertValidator(IAlertsRepository alertsRepository)
    {
        RuleFor(command => command.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(Validation.Messages.FieldRequired))
            .MustAsync(async (alertId, cancellationToken) =>
            {
                var alert = await alertsRepository.GetByIdAsync(alertId, cancellationToken);
                return alert != null;
            }).WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.Alert));

    }
}
