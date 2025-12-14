using Application.Abstractions.Services;
using Application.Models;
using Application.Models.Extensions;
using Domain.Abstractions.Events;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Clients.Create;

public class ClientCreatedEventHandler(IEventStreamingService<ClientResponse> eventStreaming, ILogger<ClientCreatedEventHandler> logger) : IEventHandler<CreatedEvent<Client>>
{
    public Task Handle(CreatedEvent<Client> createdEvent, CancellationToken cancellationToken)
    {
        eventStreaming.BroadcastAsync(new BroadcastMessage<ClientResponse>(createdEvent.Record.Adapt<ClientResponse>(), createdEvent.GetBroadcastMessageType()), cancellationToken);

        logger.LogDebug("Client creation event processed for {ClientId} at {ProcessedAt}", createdEvent.Record.Id, DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
