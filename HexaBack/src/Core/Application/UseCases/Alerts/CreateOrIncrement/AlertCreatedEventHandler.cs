using Application.Abstractions.Services;
using Application.Models;
using Application.Models.Extensions;
using Domain.Abstractions.Events;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Alerts.CreateOrIncrement;

public class AlertCreatedEventHandler(IEventStreamingService<AlertResponse> eventStreaming, ILogger<AlertCreatedEventHandler> logger) : IEventHandler<CreatedEvent<Alert>>
{
    public Task Handle(CreatedEvent<Alert> createdEvent, CancellationToken cancellationToken)
    {
        var df = createdEvent.GetBroadcastMessageType();

        eventStreaming.BroadcastAsync(new BroadcastMessage<AlertResponse>(createdEvent.Record.Adapt<AlertResponse>(), createdEvent.GetBroadcastMessageType()), cancellationToken);

        logger.LogDebug("Alert creation event processed for {AlertId} at {ProcessedAt}", createdEvent.Record.Id, DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
