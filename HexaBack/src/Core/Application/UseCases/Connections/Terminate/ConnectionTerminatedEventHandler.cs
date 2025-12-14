using Application.Abstractions.Services;
using Application.Models;
using Application.Models.Extensions;
using Domain.Abstractions.Events;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Connections.Terminate;

public class ConnectionTerminatedEventHandler(IEventStreamingService<ConnectionResponse> eventStreaming, ILogger<ConnectionTerminatedEventHandler> logger) : IEventHandler<DeletedEvent<Connection>>
{
    public Task Handle(DeletedEvent<Connection> terminatedEvent, CancellationToken cancellationToken)
    {
        eventStreaming.BroadcastAsync(new BroadcastMessage<ConnectionResponse>(terminatedEvent.Record.Adapt<ConnectionResponse>(), terminatedEvent.GetBroadcastMessageType()), cancellationToken);

        logger.LogDebug("Connection terminated event processed for {ConnectionId} at {ProcessedAt}", terminatedEvent.Record.Id, DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
