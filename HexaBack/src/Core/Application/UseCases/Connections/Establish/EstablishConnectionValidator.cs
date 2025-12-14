using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;
using MCS.WatchTower.WebApi.DataTransferObjects.Utilities;

namespace Application.UseCases.Connections.Establish;

public class EstablishConnectionValidator : AbstractValidator<EstablishConnectionCommand>
{
    public EstablishConnectionValidator(IConnectionsRepository connectionsRepository, IClientsRepository clientsRepository, IServersRepository serversRepository)
    {
        RuleFor(command => command.Request.ProcessId)
            .NotEmpty()
            .OverridePropertyName(nameof(EstablishConnectionCommand.Request.ProcessId))
            .WithMessage(Validation.Messages.FieldRequired);

        RuleFor(command => command.Request.Machine)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired);

        RuleFor(command => command.Request.ClientId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .OverridePropertyName(nameof(EstablishConnectionCommand.Request.ClientId))
            .WithMessage(Validation.Messages.FieldRequired)
            .MustAsync(async (clientId, cancellationToken) =>
            {
                var client = await clientsRepository.GetByIdAsync(clientId, cancellationToken);
                return client != null;
            })
            .WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.Client));

        RuleFor(command => command.Request.ServerId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .OverridePropertyName(nameof(EstablishConnectionCommand.Request.ServerId))
            .WithMessage(Validation.Messages.FieldRequired)
            .MustAsync(async (serverId, cancellationToken) =>
            {
                var server = await serversRepository.GetByIdAsync(serverId, cancellationToken);
                return server != null;
            })
            .WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.Server));

        RuleFor(command => command)
            .MustAsync(async (command, cancellationToken) =>
            {
                var generatedId = IdBuilder.ConnectionIdFromServerIdAndClientId(command.Request.ServerId, command.Request.ClientId);

                var existingConnection = await connectionsRepository.GetByIdAsync(generatedId, cancellationToken);
                return existingConnection == null;
            })
            .WithMessage(command => string.Format(Validation.Messages.RelationshipAlreadyExists, Validation.Entities.Connection, Validation.Entities.Client, command.Request.ClientId, Validation.Entities.Server, command.Request.ServerId))
            .When(command => !string.IsNullOrEmpty(command.Request.ClientId) && !string.IsNullOrEmpty(command.Request.ServerId));
    }
}
