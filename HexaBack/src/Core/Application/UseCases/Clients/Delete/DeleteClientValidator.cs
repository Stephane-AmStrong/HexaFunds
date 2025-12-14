using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.Clients.Delete;

public class DeleteClientValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientValidator(IClientsRepository clientsRepository)
    {
        RuleFor(command => command.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(Validation.Messages.FieldRequired))
            .MustAsync(async (clientId, cancellationToken) =>
            {
                var client = await clientsRepository.GetByIdAsync(clientId, cancellationToken);
                return client != null;
            }).WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.Client));

    }
}
