using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.Clients.Update;

public class UpdateClientValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientValidator(IClientsRepository clientsRepository, IServersRepository serversRepository)
    {
        RuleFor(command => command.Payload.Gaia)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(UpdateClientCommand.Payload.Gaia))
            .MustAsync(async (request, gaia, cancellationToken) =>
            {
                var conflictingClients = await clientsRepository.FindByConditionAsync(client => client.Gaia == gaia && client.Login != request.Payload.Login, cancellationToken);
                return conflictingClients.Count == 0;

            }).WithMessage(string.Format(Validation.Messages.FieldAlreadyInUseByAnother, nameof(UpdateClientCommand.Payload.Gaia), Validation.Entities.Client));

        RuleFor(command => command.Payload.Login)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(UpdateClientCommand.Payload.Login))
            .MustAsync(async (request, login, cancellationToken) =>
            {
                var conflictingClients = await clientsRepository.FindByConditionAsync(client => client.Login == login && client.Gaia != request.Payload.Gaia, cancellationToken);
                return conflictingClients.Count == 0;

            }).WithMessage(string.Format(Validation.Messages.FieldAlreadyInUseByAnother, nameof(UpdateClientCommand.Payload.Login), Validation.Entities.Client));
    }
}
