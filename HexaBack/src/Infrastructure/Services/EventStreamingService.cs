using System.Threading.Channels;
using Application.Abstractions.Services;
using Application.Models;
using MCS.WatchTower.WebApi.DataTransferObjects;

namespace Services;

public class EventStreamingService<TDto> : IEventStreamingService<TDto> where TDto : IBaseDto
{
    private readonly Channel<BroadcastMessage<TDto>> _channel = Channel.CreateUnbounded<BroadcastMessage<TDto>>(new UnboundedChannelOptions
    {
        SingleReader = false,
        SingleWriter = true,
        AllowSynchronousContinuations = true
    });

    public ValueTask BroadcastAsync(BroadcastMessage<TDto> domainEvent, CancellationToken cancellationToken)
    {
        var baseEvent = new BroadcastMessage<TDto>(domainEvent.Record, domainEvent.MessageType);
        return _channel.Writer.WriteAsync(baseEvent, cancellationToken);
    }

    public ChannelReader<BroadcastMessage<TDto>> Events => _channel.Reader;
}
