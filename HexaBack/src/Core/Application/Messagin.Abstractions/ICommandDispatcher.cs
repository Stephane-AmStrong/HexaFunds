namespace Application.Messagin.Abstractions;

public interface ICommandDispatcher
{
    ValueTask DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand<Unit>;

    ValueTask<TResult> DispatchAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>;
}