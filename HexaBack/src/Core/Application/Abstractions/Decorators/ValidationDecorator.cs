using Application.Abstractions.Handlers;
using Domain.Errors;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Abstractions.Decorators;

public static class ValidationDecorator
{
    public sealed class CommandHandler<TCommand, TResult>(ICommandHandler<TCommand, TResult> innerHandler, IValidator<TCommand> validator) : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateAsync(command, validator, cancellationToken);

            if (validationResult.Count > 0) throw new BadRequestException(validationResult);

            return await innerHandler.HandleAsync(command, cancellationToken);
        }
    }

    public sealed class CommandBaseHandler<TCommand>(ICommandHandler<TCommand> innerHandler, IValidator<TCommand> validator) : ICommandHandler<TCommand> where TCommand : ICommand
    {
        public async Task HandleAsync(TCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateAsync(command, validator, cancellationToken);

            if (validationResult.Count > 0) throw new BadRequestException(validationResult);

            await innerHandler.HandleAsync(command, cancellationToken);
        }
    }

    private static async Task<Dictionary<string, string[]>> ValidateAsync<TCommand>(TCommand command, IValidator<TCommand> validator, CancellationToken cancellationToken)
    {
        if (validator == null)
        {
            return [];
        }

        var context = new ValidationContext<TCommand>(command);

        ValidationResult validationResult = await validator.ValidateAsync(context, cancellationToken);

        return validationResult.Errors.GroupBy(x => x.PropertyName).ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());
    }
}
