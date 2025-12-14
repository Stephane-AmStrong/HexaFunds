using Application.Abstractions.Services;
using Application.Models;
using Application.Models.Extensions;
using Domain.Abstractions.Events;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Servers.Delete;

public class ServerDeletedEventHandler(IEventStreamingService<ServerResponse> eventStreaming, ILogger<ServerDeletedEventHandler> logger) : IEventHandler<DeletedEvent<Server>>
{
    public Task Handle(DeletedEvent<Server> deletedEvent, CancellationToken cancellationToken)
    {
        eventStreaming.BroadcastAsync(new BroadcastMessage<ServerResponse>(deletedEvent.Record.Adapt<ServerResponse>(), deletedEvent.GetBroadcastMessageType()), cancellationToken);

        logger.LogDebug("Server deletion event processed for {ServerId} at {ProcessedAt}", deletedEvent.Record.Id, DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
