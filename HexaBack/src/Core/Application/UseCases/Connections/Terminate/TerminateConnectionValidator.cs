using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.Connections.Terminate;

public class TerminateConnectionValidator : AbstractValidator<TerminateConnectionCommand>
{
    public TerminateConnectionValidator(IConnectionsRepository connectionsRepository)
    {
        RuleFor(command => command.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(Validation.Messages.FieldRequired))
            .MustAsync(async (connectionId, cancellationToken) =>
            {
                var connection = await connectionsRepository.GetByIdAsync(connectionId, cancellationToken);
                return connection != null;
            }).WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.Connection));

    }
}