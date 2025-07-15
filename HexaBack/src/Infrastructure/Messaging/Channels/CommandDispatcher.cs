using System.Threading.Channels;
using Application.Messagin.Abstractions;
using Messaging.Internals;

namespace Messaging.Channels;

public class CommandDispatcher(Channel<ICommandWrapper> channel) : ICommandDispatcher
{
    public ValueTask DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand<Unit>
    {
        var wrapper = new CommandWrapper<TCommand, Unit>(command);
        return channel.Writer.WriteAsync(wrapper);
    }

    public ValueTask<TResult> DispatchAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>
    {
        var wrapper = new CommandWrapper<TCommand, TResult>(command);
        channel.Writer.TryWrite(wrapper);
        return wrapper.CompletionSource;
    }
}

/*
public class ChannelCommandDispatcher(Channel<ICommandWrapper> channel) : ICommandDispatcher
{
    public ValueTask DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
        var wrapper = new CommandWrapper<TCommand, Unit>(command as TCommand);
        return channel.Writer.WriteAsync(wrapper);
    }

    public Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>
    {
        var wrapper = new CommandWrapper<TCommand, TResult>(command);
        channel.Writer.TryWrite(wrapper);
        return wrapper.CompletionSource.Task;
    }
}


*/

/*
public class ChannelCommandDispatcher(Channel<ICommandWrapper> channel) : ICommandDispatcher
{
    public ValueTask DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
        var wrapper = new CommandWrapper<TCommand, Unit>(command as TCommand);
        return channel.Writer.WriteAsync(wrapper);
    }

    public ValueTask<TResult> DispatchAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>
    {
        var wrapper = new CommandWrapper<TCommand, TResult>(command);
        channel.Writer.TryWrite(wrapper);
        return wrapper.CompletionSource;
    }
}

*/