using Application.Common;
using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.UseCases.Hosts.Update;

public class UpdateHostValidator : AbstractValidator<UpdateHostCommand>
{
    public UpdateHostValidator(IHostsRepository hostsRepository)
    {
        RuleFor(host => host.Payload.Name)
            .NotEmpty()
            .OverridePropertyName(nameof(UpdateHostCommand.Payload.Name))
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(UpdateHostCommand.Payload.Name))
            .MustAsync(async (request, name, cancellationToken) =>
            {
                var existingHosts = (await hostsRepository.FindByConditionAsync(host => host.Name == name && host.Id != request.Id, cancellationToken));
                return existingHosts.Count != 0;

            }).WithMessage(string.Format(Validation.Messages.FieldAlreadyInUseByAnother, nameof(UpdateHostCommand.Payload.Name), Validation.Entities.Host));

        RuleFor(command => command.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(Validation.Messages.FieldRequired)
            .OverridePropertyName(nameof(UpdateHostCommand.Id))
            .MustAsync(async (request, id, cancellationToken) =>
            {
                var existingHost = await hostsRepository.GetByIdAsync(id, cancellationToken);
                return existingHost is not null;

            }).WithMessage(string.Format(Validation.Messages.FieldAlreadyInUseByAnother, nameof(UpdateHostCommand.Id), Validation.Entities.Host));
    }
}
