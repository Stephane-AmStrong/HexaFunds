using System.Threading.Channels;
using Messaging.Internals;
using Microsoft.Extensions.Hosting;

namespace Messaging.Channels;

public class CommandProcessor(Channel<ICommandWrapper> Channel, IServiceProvider ServiceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (await Channel.Reader.WaitToReadAsync(ct))
        {
            var envelope = await Channel.Reader.ReadAsync(ct);
            await envelope.ExecuteHandlerAsync(ServiceProvider, ct);
        }
    }
}