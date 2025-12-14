using Application.Abstractions.Services;
using Application.Models;
using Application.Models.Extensions;
using Domain.Abstractions.Events;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Hosts.Delete;

public class HostDeletedEventHandler(IEventStreamingService<HostResponse> eventStreaming, ILogger<HostDeletedEventHandler> logger) : IEventHandler<DeletedEvent<Host>>
{
    public Task Handle(DeletedEvent<Host> deletedEvent, CancellationToken cancellationToken)
    {
        eventStreaming.BroadcastAsync(new BroadcastMessage<HostResponse>(deletedEvent.Record.Adapt<HostResponse>(), deletedEvent.GetBroadcastMessageType()), cancellationToken);

        logger.LogDebug("Host deletion event processed for {HostId} at {ProcessedAt}", deletedEvent.Record.Id, DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
