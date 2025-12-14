using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.Alerts.CreateOrIncrement;

public class CreateOrIncrementAlertValidator : AbstractValidator<CreateOrIncrementAlertCommand>
{
    public CreateOrIncrementAlertValidator(IServersRepository serversRepository)
    {
        RuleFor(command => command.Payload.Type)
            .NotNull()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateOrIncrementAlertCommand.Payload.Type));

        RuleFor(command => command.Payload.Severity)
            .NotNull()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateOrIncrementAlertCommand.Payload.Severity));

        RuleFor(command => command.Payload.ServerId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateOrIncrementAlertCommand.Payload.ServerId))
            .MustAsync(async (serverId, cancellationToken) =>
            {
                var server = await serversRepository.GetByIdAsync(serverId, cancellationToken);
                return server is not null;
            })
            .WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.Server));
    }
}
