using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.Servers.Delete;

public class DeleteServerValidator : AbstractValidator<DeleteServerCommand>
{
    public DeleteServerValidator(IServersRepository serversRepository)
    {
        RuleFor(command => command.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(Validation.Messages.FieldRequired))
            .MustAsync(async (serverId, cancellationToken) =>
            {
                var server = await serversRepository.GetByIdAsync(serverId, cancellationToken);
                return server != null;
            }).WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.Server));

    }
}
