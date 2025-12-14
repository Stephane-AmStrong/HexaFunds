using Application.Abstractions.Services;
using Application.Models;
using Application.Models.Extensions;
using Domain.Abstractions.Events;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ServerStatuses.Create;

public class ServerStatusCreatedEventHandler(IEventStreamingService<ServerStatusResponse> eventStreaming, ILogger<ServerStatusCreatedEventHandler> logger) : IEventHandler<CreatedEvent<ServerStatus>>
{
    public Task Handle(CreatedEvent<ServerStatus> createdEvent, CancellationToken cancellationToken)
    {
        eventStreaming.BroadcastAsync(new BroadcastMessage<ServerStatusResponse>(createdEvent.Record.Adapt<ServerStatusResponse>(), createdEvent.GetBroadcastMessageType()), cancellationToken);

        logger.LogDebug("ServerStatus creation event processed for {ServerStatusId} at {ProcessedAt}", createdEvent.Record.Id, DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
