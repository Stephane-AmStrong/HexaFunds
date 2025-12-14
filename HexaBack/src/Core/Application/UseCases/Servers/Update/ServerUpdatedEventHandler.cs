using Application.Abstractions.Services;
using Application.Common;
using Application.Models;
using Application.Models.Extensions;
using Domain.Abstractions.Events;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Servers.Update;

public class ServerUpdatedEventHandler(IEventStreamingService<ServerResponse> eventStreaming, IHeartbeatNotificationService heartbeatNotificationService, ILogger<ServerUpdatedEventHandler> logger) : IEventHandler<UpdatedEvent<Server>>
{
    public Task Handle(UpdatedEvent<Server> updatedEvent, CancellationToken cancellationToken)
    {
        eventStreaming.BroadcastAsync(new BroadcastMessage<ServerResponse>(updatedEvent.Record.Adapt<ServerResponse>(), updatedEvent.GetBroadcastMessageType()), cancellationToken);

        logger.LogDebug("Server update event processed for {ServerId} at {ProcessedAt}", updatedEvent.Record.Id, DateTime.UtcNow);

        heartbeatNotificationService.OnServerCreatedOrUpdated(updatedEvent.Record.Id);

        return Task.CompletedTask;
    }
}
