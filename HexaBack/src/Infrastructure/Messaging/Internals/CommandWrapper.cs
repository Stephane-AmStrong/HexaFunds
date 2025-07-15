using Application.Messagin.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Internals;

public class CommandWrapper<TCommand, TResult> : ICommandWrapper where TCommand : ICommand<TResult>
{
    public TCommand Command { get; }
    private readonly TaskCompletionSource<TResult> _completionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public ValueTask<TResult> CompletionSource => new(_completionSource.Task);

    public ICommand BaseCommand => Command;

    public CommandWrapper(TCommand command)
    {
        Command = command;
    }

    public async Task ExecuteHandlerAsync(IServiceProvider sp, CancellationToken ct)
    {
        var handler = sp.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        var result = await handler.HandleAsync(Command, ct);
        _completionSource.SetResult(result);
    }
}