using Application.Common;
using Domain.Abstractions.Repositories;
using Domain.Shared;
using FluentValidation;

namespace Application.UseCases.Hosts.Create;

public class CreateHostValidator : AbstractValidator<CreateHostCommand>
{
    public CreateHostValidator(IHostsRepository hostsRepository)
    {
        RuleFor(command => command.Payload.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(CreateHostCommand.Payload.Name))
            .MustAsync(async (name, cancellationToken) =>
            {
                var conflictingHosts = (await hostsRepository.FindByConditionAsync(host => host.Name == name, cancellationToken));
                return conflictingHosts.Count == 0;
            })
            .WithMessage(string.Format(Validation.Messages.IdAlreadyInUse, Validation.Entities.Host));
    }
}
