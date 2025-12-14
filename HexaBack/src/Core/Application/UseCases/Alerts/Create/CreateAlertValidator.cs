using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.Alerts.Create;

public class CreateAlertValidator : AbstractValidator<CreateAlertCommand>
{
    public CreateAlertValidator(IServersRepository serversRepository)
    {
        RuleFor(command => command.Payload.Type)
            .NotNull()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateAlertCommand.Payload.Type));

        RuleFor(command => command.Payload.Severity)
            .NotNull()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateAlertCommand.Payload.Severity));

        RuleFor(command => command.Payload.OccurredAt)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateAlertCommand.Payload.OccurredAt));

        RuleFor(command => command.Payload.ServerId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateAlertCommand.Payload.ServerId))
            .MustAsync(async (serverId, cancellationToken) =>
            {
                var server = await serversRepository.GetByIdAsync(serverId, cancellationToken);
                return server is not null;
            })
            .WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.Server));
    }
}
