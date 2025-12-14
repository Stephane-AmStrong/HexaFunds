#nullable enable
using Application.Abstractions.Services;
using Application.UseCases.ServerStatuses.GetByQuery;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared.Common;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Services;

public sealed class ServerStatusesService(IServerStatusesRepository serverStatusesRepository, IServersRepository serversRepository, IHeartbeatNotificationService heartbeatNotificationService, ILogger<ServerStatusesService> logger) : IServerStatusesService
{
    public async Task<PagedList<ServerStatusResponse>> GetPagedListByQueryAsync(ServerStatusQuery query, CancellationToken cancellationToken)
    {
        logger.LogDebug("Getting ServerStatuses with query parameters: {@QueryParameters}", query);
        var serverStatuses = await serverStatusesRepository.GetPagedListByQueryAsync(query, cancellationToken);

        var serverStatusResponses = serverStatuses.Adapt<List<ServerStatusResponse>>();

        logger.LogDebug("Retrieved ServerStatuses with meta data: {@MetaData}", serverStatuses.MetaData);
        return new PagedList<ServerStatusResponse>(serverStatusResponses, serverStatuses.MetaData);
    }

    public async Task<ServerStatusDetailedResponse?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogDebug("Retrieving serverStatus with ID: {ServerStatusId}", id);
        ServerStatus serverStatus = await serverStatusesRepository.GetByIdAsync(id, cancellationToken) ?? throw new ServerStatusNotFoundException(id);

        Server? server = await serversRepository.GetByIdAsync(serverStatus.ServerId, cancellationToken);

        if (server is null) logger.LogWarning("ServerStatus {ServerStatusId} refers to a missing server (ServerId: {ServerId})", id, serverStatus.ServerId);

        logger.LogDebug("ServerStatus {ServerStatusId} retrieved.", id);

        return serverStatus.Adapt<ServerStatusDetailedResponse>() with
        {
            Server = server?.Adapt<ServerResponse>()
        };
    }

    public async Task<ServerStatusResponse> CreateAsync(ServerStatusCreateRequest serverStatusRequest, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing ServerStatus creation for ServerId={ServerId} and Status={ServerStatusStatus}",
            serverStatusRequest.ServerId, serverStatusRequest.Status);

        var lastServerStatus = (await serverStatusesRepository.GetLatestByServerIdsAsync(new HashSet<string>([serverStatusRequest.ServerId!]), cancellationToken)).FirstOrDefault();

        heartbeatNotificationService.OnServerStatusReceived(serverStatusRequest.ServerId, serverStatusRequest.Status ?? MCS.WatchTower.WebApi.DataTransferObjects.Enumerations.ServerStatus.Up);

        return lastServerStatus is { Status: var lastStatus } && lastStatus == serverStatusRequest.Status.ToString()
            ? lastServerStatus.Adapt<ServerStatusResponse>()
            : await CreateNewServerStatus(serverStatusRequest, cancellationToken);
    }

    private async Task<ServerStatusResponse> CreateNewServerStatus(ServerStatusCreateRequest serverStatusRequest, CancellationToken cancellationToken)
    {
        logger.LogDebug("Creating new ServerStatus of status: {ServerStatusStatus} for serverId: {ServerId}", serverStatusRequest.Status, serverStatusRequest.ServerId);

        var serverStatus = serverStatusRequest.Adapt<ServerStatus>() with
        {
            RecordedAt = DateTime.UtcNow,
        };

        await serverStatusesRepository.CreateAsync(serverStatus, cancellationToken);

        logger.LogDebug("Successfully created ServerStatus with ID: {ServerStatusId}", serverStatus.Id);

        return serverStatus.Adapt<ServerStatusResponse>();
    }
}
