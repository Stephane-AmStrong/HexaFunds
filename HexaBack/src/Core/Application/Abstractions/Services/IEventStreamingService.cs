using System.Threading.Channels;
using Application.Models;
using MCS.WatchTower.WebApi.DataTransferObjects;

namespace Application.Abstractions.Services;

public interface IEventStreamingService<T> where T : IBaseDto
{
    ValueTask BroadcastAsync(BroadcastMessage<T> evt, CancellationToken cancellationToken);

    ChannelReader<BroadcastMessage<T>> Events { get; }
}
