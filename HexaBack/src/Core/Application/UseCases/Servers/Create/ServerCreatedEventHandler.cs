using Application.Abstractions.Services;
using Application.Common;
using Application.Models;
using Application.Models.Extensions;
using Domain.Abstractions.Events;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Servers.Create;

public class ServerCreatedEventHandler(IEventStreamingService<ServerResponse> eventStreaming, IHeartbeatNotificationService heartbeatNotificationService, ILogger<ServerCreatedEventHandler> logger) : IEventHandler<CreatedEvent<Server>>
{
    public Task Handle(CreatedEvent<Server> createdEvent, CancellationToken cancellationToken)
    {
        eventStreaming.BroadcastAsync(new BroadcastMessage<ServerResponse>(createdEvent.Record.Adapt<ServerResponse>(), createdEvent.GetBroadcastMessageType()), cancellationToken);

        logger.LogDebug("Server creation event processed for {ServerId} at {ProcessedAt}", createdEvent.Record.Id, DateTime.UtcNow);

        heartbeatNotificationService.OnServerCreatedOrUpdated(createdEvent.Record.Id);

        return Task.CompletedTask;
    }
}
