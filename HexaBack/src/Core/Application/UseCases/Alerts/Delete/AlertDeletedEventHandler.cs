using Application.Abstractions.Services;
using Application.Models;
using Application.Models.Extensions;
using Domain.Abstractions.Events;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Alerts.Delete;

public class AlertDeletedEventHandler(IEventStreamingService<AlertResponse> eventStreaming, ILogger<AlertDeletedEventHandler> logger) : IEventHandler<DeletedEvent<Alert>>
{
    public Task Handle(DeletedEvent<Alert> deletedEvent, CancellationToken cancellationToken)
    {
        eventStreaming.BroadcastAsync(new BroadcastMessage<AlertResponse>(deletedEvent.Record.Adapt<AlertResponse>(), deletedEvent.GetBroadcastMessageType()), cancellationToken);

        logger.LogDebug("Alert deletion event processed for {AlertId} at {ProcessedAt}", deletedEvent.Record.Id, DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
