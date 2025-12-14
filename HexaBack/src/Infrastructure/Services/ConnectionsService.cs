#nullable enable
using Application.Abstractions.Services;
using Application.UseCases.Connections.GetByQuery;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared.Common;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using MCS.WatchTower.WebApi.DataTransferObjects.Utilities;
using Microsoft.Extensions.Logging;

namespace Services;

public sealed class ConnectionsService(
    IConnectionsRepository connectionsRepository,
    IServersRepository serversRepository,
    IClientsRepository clientsRepository,
    ILogger<ConnectionsService> logger
    ) : IConnectionsService
{
    public async Task<PagedList<ConnectionResponse>> GetPagedListByQueryAsync(ConnectionQuery queryParameters, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving connections with query parameters: {@QueryParameters}", queryParameters);
        var connections = await connectionsRepository.GetPagedListByQueryAsync(queryParameters, cancellationToken);

        var connectionResponses = connections.Adapt<List<ConnectionResponse>>();

        logger.LogInformation("Retrieved connections with meta data: {@MetaData}", connections.MetaData);
        return new PagedList<ConnectionResponse>(connectionResponses, connections.MetaData);
    }

    public async Task<ConnectionDetailedResponse?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving connection with ID: {ConnectionId}", id);
        var connection = await connectionsRepository.GetByIdAsync(id, cancellationToken) ?? throw new ConnectionNotFoundException(id);

        var server = await serversRepository.GetByIdAsync(connection.ServerId, cancellationToken);
        var client = await clientsRepository.GetByIdAsync(connection.ClientId, cancellationToken);

        if (server is null) logger.LogWarning("Connection {ConnectionId} refers to a missing server (ServerId: {ServerId})", id, connection.ServerId);
        if (client is null) logger.LogWarning("Connection {ConnectionId} refers to a missing client (ClientId: {ClientId})", id, connection.ClientId);

        var connectionDetailedResponse = connection.Adapt<ConnectionDetailedResponse>();

        logger.LogInformation("Successfully retrieved connection {ConnectionId} with server and client references", id);

        return connectionDetailedResponse with
        {
            Server = server.Adapt<ServerResponse>(),
            Client = client.Adapt<ClientResponse>()
        };
    }

    public async Task<ConnectionResponse> EstablishAsync(ConnectionEstablishRequest connectionRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Establishing new connection with serverId: {ServerId} and clientId: {ClientId}", connectionRequest.ServerId, connectionRequest.ClientId);

        var connection = connectionRequest.Adapt<Connection>() with
        {
            Id = IdBuilder.ConnectionIdFromServerIdAndClientId(connectionRequest.ServerId, connectionRequest.ClientId)
        };

        await connectionsRepository.EstablishAsync(connection, cancellationToken);

        logger.LogInformation("Successfully established connection with ID: {ConnectionId}", connection.Id);
        return connection.Adapt<ConnectionResponse>();
    }

    public async Task TerminateAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Terminating connection with ID: {ConnectionId}", id);

        var connection = await connectionsRepository.GetByIdAsync(id, cancellationToken) ?? throw new ConnectionNotFoundException(id);

        await connectionsRepository.TerminateAsync(connection, cancellationToken);

        logger.LogInformation("Successfully terminated connection with ID: {ConnectionId}", id);
    }
}
