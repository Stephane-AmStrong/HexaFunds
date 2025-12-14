using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.ServerStatuses.Create;

public class CreateServerStatusValidator : AbstractValidator<CreateServerStatusCommand>
{
    public CreateServerStatusValidator(IServersRepository serversRepository)
    {
        RuleFor(command => command.Payload.Status)
            .NotNull()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateServerStatusCommand.Payload.Status));

        RuleFor(command => command.Payload.ServerId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateServerStatusCommand.Payload.ServerId))
            .MustAsync(async (serverId, cancellationToken) =>
            {
                var server = await serversRepository.GetByIdAsync(serverId, cancellationToken);
                return server is not null;
            })
            .WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.Server));
    }
}
