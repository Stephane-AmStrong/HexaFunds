using Application.Abstractions.Services;
using Application.Models;
using Application.Models.Extensions;
using Domain.Abstractions.Events;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Connections.Establish;

public class ConnectionEstablishedEventHandler(IEventStreamingService<ConnectionResponse> eventStreaming, ILogger<ConnectionEstablishedEventHandler> logger) : IEventHandler<CreatedEvent<Connection>>
{
    public Task Handle(CreatedEvent<Connection> establishEvent, CancellationToken cancellationToken)
    {
        eventStreaming.BroadcastAsync(new BroadcastMessage<ConnectionResponse>(establishEvent.Record.Adapt<ConnectionResponse>(), establishEvent.GetBroadcastMessageType()), cancellationToken);

        logger.LogDebug("Connection creation event processed for {ConnectionId} at {ProcessedAt}", establishEvent.Record.Id, DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
