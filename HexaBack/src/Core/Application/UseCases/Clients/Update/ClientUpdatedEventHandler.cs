using Application.Abstractions.Services;
using Application.Models;
using Application.Models.Extensions;
using Domain.Abstractions.Events;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Clients.Update;

public class ClientUpdatedEventHandler(IEventStreamingService<ClientResponse> eventStreaming, ILogger<ClientUpdatedEventHandler> logger) : IEventHandler<UpdatedEvent<Client>>
{
    public Task Handle(UpdatedEvent<Client> updatedEvent, CancellationToken cancellationToken)
    {
        eventStreaming.BroadcastAsync(new BroadcastMessage<ClientResponse>(updatedEvent.Record.Adapt<ClientResponse>(), updatedEvent.GetBroadcastMessageType()), cancellationToken);

        logger.LogDebug("Client update event processed for {ClientId} at {ProcessedAt}", updatedEvent.Record.Id, DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
