#nullable enable
using Application.Abstractions.Services;
using Application.UseCases.Clients.GetByQuery;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared.Common;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Services;

public sealed class ClientsService(
    IClientsRepository clientsRepository,
    IConnectionsRepository connectionsRepository,
    ILogger<ClientsService> logger
    ) : IClientsService
{
    public async Task<PagedList<ClientResponse>> GetPagedListByQueryAsync(ClientQuery queryParameters, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving clients with query parameters: {@QueryParameters}", queryParameters);
        var clients = await clientsRepository.GetPagedListByQueryAsync(queryParameters, cancellationToken);

        var clientResponses = clients.Adapt<List<ClientResponse>>();

        logger.LogInformation("Retrieved clients with meta data: {@MetaData}", clients.MetaData);
        return new PagedList<ClientResponse>(clientResponses, clients.MetaData);
    }

    public async Task<ClientDetailedResponse?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving client with ID: {ClientId}", id);
        var client = await clientsRepository.GetByIdAsync(id, cancellationToken) ?? throw new ClientNotFoundException(id);

        List<Connection> connections = await connectionsRepository.FindByConditionAsync(connection => connection.ClientId == id, cancellationToken);
        logger.LogDebug("Found {ConnectionCount} connections for client {ClientId}", connections.Count, id);

        var clientDetailedResponse = client.Adapt<ClientDetailedResponse>();

        logger.LogInformation("Successfully retrieved client {ClientId} with {ConnectionCount} connections", id, connections.Count);
        return clientDetailedResponse with
        {
            Connections = connections.Adapt<IList<ConnectionResponse>>()
        };
    }

    public async Task<ClientResponse> CreateAsync(ClientCreateRequest clientRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new client with gaia: {ClientGaia}", clientRequest.Gaia);
        var client = clientRequest.Adapt<Client>();

        await clientsRepository.CreateAsync(client, cancellationToken);

        logger.LogInformation("Successfully created client with ID: {ClientId}", client.Id);
        return client.Adapt<ClientResponse>();
    }

    public async Task UpdateAsync(string id, ClientUpdateRequest clientRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating client with ID: {ClientId}", id);

        var client = await clientsRepository.GetByIdAsync(id, cancellationToken) ?? throw new ClientNotFoundException(id);

        clientRequest.Adapt(client);

        logger.LogDebug("Applying updates to client {ClientId}: HostName={NewName}, AppName={NewAppName}", id, clientRequest.Gaia, clientRequest.Login);
        await clientsRepository.UpdateAsync(client, cancellationToken);

        logger.LogInformation("Successfully updated client with ID: {ClientId}", id);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting client with ID: {ClientId}", id);

        var client = await clientsRepository.GetByIdAsync(id, cancellationToken) ?? throw new ClientNotFoundException(id);

        await clientsRepository.DeleteAsync(client, cancellationToken);

        logger.LogInformation("Successfully deleted client with ID: {ClientId}", id);
    }
}
