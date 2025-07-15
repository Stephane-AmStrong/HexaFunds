namespace Application.Messagin.Abstractions;

public interface ICommand;
public interface ICommand<TResult> : ICommand;

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    ValueTask<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}