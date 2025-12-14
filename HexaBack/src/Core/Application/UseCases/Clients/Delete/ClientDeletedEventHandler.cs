using Application.Abstractions.Services;
using Application.Models;
using Application.Models.Extensions;
using Domain.Abstractions.Events;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Clients.Delete;

public class ClientDeletedEventHandler(IEventStreamingService<ClientResponse> eventStreaming, ILogger<ClientDeletedEventHandler> logger) : IEventHandler<DeletedEvent<Client>>
{
    public Task Handle(DeletedEvent<Client> deletedEvent, CancellationToken cancellationToken)
    {
        eventStreaming.BroadcastAsync(new BroadcastMessage<ClientResponse>(deletedEvent.Record.Adapt<ClientResponse>(), deletedEvent.GetBroadcastMessageType()), cancellationToken);

        logger.LogDebug("Client deletion event processed for {ClientId} at {ProcessedAt}", deletedEvent.Record.Id, DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
