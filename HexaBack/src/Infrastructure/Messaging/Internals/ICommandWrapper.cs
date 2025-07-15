using Application.Messagin.Abstractions;

namespace Messaging.Internals;

public interface ICommandWrapper
{
    ICommand BaseCommand { get; }
    Task ExecuteHandlerAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken);
}