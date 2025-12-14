using Application.Abstractions.Services;
using Application.Models;
using Application.Models.Extensions;
using Domain.Abstractions.Events;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Alerts.Update;

public class AlertUpdatedEventHandler(IEventStreamingService<AlertResponse> eventStreaming, ILogger<AlertUpdatedEventHandler> logger) : IEventHandler<UpdatedEvent<Alert>>
{
    public Task Handle(UpdatedEvent<Alert> updatedEvent, CancellationToken cancellationToken)
    {
        eventStreaming.BroadcastAsync(new BroadcastMessage<AlertResponse>(updatedEvent.Record.Adapt<AlertResponse>(), updatedEvent.GetBroadcastMessageType()), cancellationToken);

        logger.LogDebug("Alert update event processed for {AlertId} at {ProcessedAt}", updatedEvent.Record.Id, DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
