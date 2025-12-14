using Application.Abstractions.Services;
using Application.Common;
using Application.Models;
using Application.Models.Extensions;
using Domain.Abstractions.Events;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Hosts.Create;

public class HostCreatedEventHandler(IEventStreamingService<HostResponse> eventStreaming, ILogger<HostCreatedEventHandler> logger) : IEventHandler<CreatedEvent<Host>>
{
    public Task Handle(CreatedEvent<Host> createdEvent, CancellationToken cancellationToken)
    {
        eventStreaming.BroadcastAsync(new BroadcastMessage<HostResponse>(createdEvent.Record.Adapt<HostResponse>(), createdEvent.GetBroadcastMessageType()), cancellationToken);

        logger.LogDebug("Host creation event processed for {HostId} at {ProcessedAt}", createdEvent.Record.Id, DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
