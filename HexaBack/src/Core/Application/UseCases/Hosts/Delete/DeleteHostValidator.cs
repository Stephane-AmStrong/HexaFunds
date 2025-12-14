using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.Hosts.Delete;

public class DeleteHostValidator : AbstractValidator<DeleteHostCommand>
{
    public DeleteHostValidator(IHostsRepository hostsRepository)
    {
        RuleFor(command => command.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(Validation.Messages.FieldRequired))
            .MustAsync(async (hostId, cancellationToken) =>
            {
                var host = await hostsRepository.GetByIdAsync(hostId, cancellationToken);
                return host != null;
            }).WithMessage(string.Format(Validation.Messages.EntityNotFound, Validation.Entities.Host));
    }
}
