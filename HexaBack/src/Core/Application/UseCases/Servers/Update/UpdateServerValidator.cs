using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.Servers.Update;

public class UpdateServerValidator : AbstractValidator<UpdateServerCommand>
{
    public UpdateServerValidator(IServersRepository serversRepository)
    {
        RuleFor(server => server.Payload.HostName)
            .NotEmpty()
            .OverridePropertyName(nameof(UpdateServerCommand.Payload.HostName))
            .WithMessage(Validation.Messages.FieldRequired);

        RuleFor(server => server.Payload.AppName)
            .NotEmpty()
            .OverridePropertyName(nameof(UpdateServerCommand.Payload.AppName))
            .WithMessage(Validation.Messages.FieldRequired);

        RuleFor(command => command.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(UpdateServerCommand.Id))
            .MustAsync(async (request, id, cancellationToken) =>
            {
                var existingServer = await serversRepository.GetByIdAsync(id, cancellationToken);
                return existingServer is not null;

            }).WithMessage(string.Format(Validation.Messages.FieldAlreadyInUseByAnother, nameof(UpdateServerCommand.Id), Validation.Entities.Server));
    }
}
