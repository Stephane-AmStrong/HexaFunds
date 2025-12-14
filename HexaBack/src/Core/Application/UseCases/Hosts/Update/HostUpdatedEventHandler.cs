using Application.Abstractions.Services;
using Application.Common;
using Application.Models;
using Application.Models.Extensions;
using Domain.Abstractions.Events;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Hosts.Update;

public class HostUpdatedEventHandler(IEventStreamingService<HostResponse> eventStreaming, ILogger<HostUpdatedEventHandler> logger) : IEventHandler<UpdatedEvent<Host>>
{
    public Task Handle(UpdatedEvent<Host> updatedEvent, CancellationToken cancellationToken)
    {
        eventStreaming.BroadcastAsync(new BroadcastMessage<HostResponse>(updatedEvent.Record.Adapt<HostResponse>(), updatedEvent.GetBroadcastMessageType()), cancellationToken);

        logger.LogDebug("Host update event processed for {HostId} at {ProcessedAt}", updatedEvent.Record.Id, DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
