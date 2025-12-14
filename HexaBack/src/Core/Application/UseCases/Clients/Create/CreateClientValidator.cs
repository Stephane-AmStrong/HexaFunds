using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.Clients.Create;

public class CreateClientValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientValidator(IClientsRepository clientsRepository, IServersRepository serversRepository)
    {
        RuleFor(command => command.Payload.Gaia)
            .NotEmpty()
            .OverridePropertyName(nameof(CreateClientCommand.Payload.Gaia))
            .WithMessage(Validation.Messages.FieldRequired);

        RuleFor(command => command.Payload.Login)
            .NotEmpty()
            .OverridePropertyName(nameof(CreateClientCommand.Payload.Login))
            .WithMessage(Validation.Messages.FieldRequired);

        RuleFor(command => command.Payload.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateClientCommand.Payload.Id))
            .MustAsync(async (clientId, cancellationToken) =>
            {
                var client = await clientsRepository.GetByIdAsync(clientId, cancellationToken);
                return client is null;
            })
            .WithMessage(string.Format(Validation.Messages.IdAlreadyInUse, Validation.Entities.Client));
    }
}
