using Application.Common;
using Domain.Abstractions.Repositories;
using Domain.Shared;
using FluentValidation;
using MCS.WatchTower.WebApi.DataTransferObjects.Utilities;

namespace Application.UseCases.Servers.Create;

public class CreateServerValidator : AbstractValidator<CreateServerCommand>
{
    public CreateServerValidator(IServersRepository serversRepository)
    {
        RuleFor(command => command.Payload.HostName)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateServerCommand.Payload.HostName));

        RuleFor(command => command.Payload.AppName)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateServerCommand.Payload.AppName));

        RuleFor(command => command.Payload)
            .MustAsync(async (serverCreateRequest, cancellationToken) =>
            {
                var generatedId = IdBuilder.ServerIdFromHostAndApp(serverCreateRequest.HostName, serverCreateRequest.AppName);
                var existingServer = await serversRepository.GetByIdAsync(generatedId, cancellationToken);
                return existingServer == null;
            })
            .WithMessage(command => string.Format(Validation.Messages.Field1AndField2AlreadyInUse, nameof(command.Payload.HostName), command.Payload.HostName, nameof(command.Payload.AppName), command.Payload.AppName))
            .When(command => !string.IsNullOrEmpty(command.Payload.HostName) && !string.IsNullOrEmpty(command.Payload.AppName));
    }
}
