using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.Alerts.Update;

public class UpdateAlertValidator : AbstractValidator<UpdateAlertCommand>
{
    public UpdateAlertValidator(IAlertsRepository alertsRepository, IServersRepository serversRepository)
    {
        RuleFor(command => command.Payload.Type)
            .NotNull()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(UpdateAlertCommand.Payload.Type));

        RuleFor(command => command.Payload.Severity)
            .NotNull()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(UpdateAlertCommand.Payload.Severity));

        RuleFor(command => command.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(Validation.Messages.FieldRequired))
            .MustAsync(async (alertId, cancellationToken) =>
            {
                var alert = await alertsRepository.GetByIdAsync(alertId, cancellationToken);
                return alert is not null;
            }).WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.Alert));

        RuleFor(command => command.Payload.ServerId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(UpdateAlertCommand.Payload.ServerId))
            .MustAsync(async (serverId, cancellationToken) =>
            {
                var server = await serversRepository.GetByIdAsync(serverId, cancellationToken);
                return server is not null;
            })
            .WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.Server));
    }
}
