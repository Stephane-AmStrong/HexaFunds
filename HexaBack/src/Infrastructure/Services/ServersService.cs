#nullable enable
using System.Linq.Expressions;
using Application.Abstractions.Services;
using Application.UseCases.ServerStatuses.GetByQuery;
using Application.UseCases.Servers.GetByQuery;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared.Common;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using MCS.WatchTower.WebApi.DataTransferObjects.Utilities;
using Microsoft.Extensions.Logging;

namespace Services;

public sealed class ServersService(IHostsService hostsService, IServersRepository serversRepository, IConnectionsRepository connectionsRepository, IServerStatusesRepository serverStatusesRepository, ILogger<ServersService> logger) : IServersService
{
    public async Task<PagedList<ServerResponse>> GetPagedListByQueryAsync(ServerQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting servers with query parameters: {@QueryParameters}", query);
        var servers = await serversRepository.GetPagedListByQueryAsync(query, cancellationToken);

        var serverIds = new HashSet<string>(servers.Select(server => server.Id),  StringComparer.OrdinalIgnoreCase);
        var serverStatuses = await serverStatusesRepository.GetLatestByServerIdsAsync(serverIds, cancellationToken);

        logger.LogDebug("Found {ServerStatusCount} serverStatuses for {ServerIdCount} server IDs", serverStatuses.Count, serverIds.Count);
        var serverStatusesByServerId = serverStatuses.ToDictionary(p => p.ServerId, p => p.Adapt<ServerStatusResponse>());

        var serverResponses = servers.Select(server =>
        {
            var serverResponse = server.Adapt<ServerResponse>();
            var hasServerStatus = serverStatusesByServerId.TryGetValue(server.Id, out var lastServerStatus);

            if (!hasServerStatus)
            {
                logger.LogDebug("No serverStatus found for server {ServerId}", server.Id);
            }

            return serverResponse with
            {
                LatestStatus = lastServerStatus
            };
        }).ToList();

        var serversWithServerStatus = serverResponses.Count(s => s.LatestStatus != null);

        logger.LogInformation("Retrieved {ServerCount} servers ({WithServerStatusCount} with serverStatus, {WithoutServerStatusCount} without serverStatus). Meta data: {@MetaData}",
            serverResponses.Count, serversWithServerStatus, serverResponses.Count - serversWithServerStatus, servers.MetaData);

        return new PagedList<ServerResponse>(serverResponses, servers.MetaData);
    }

    public async Task<List<ServerResponse>> FindByConditionAsync(Expression<Func<ServerResponse, bool>> expression, CancellationToken cancellationToken)
    {
        logger.LogInformation("Finding servers by condition: {@Expression}", expression.ToString());

        var serverExpression = ConvertExpressionToServerType(expression);

        var servers = await serversRepository.FindByConditionAsync(serverExpression, cancellationToken);

        if (!servers.Any())
        {
            logger.LogInformation("No servers found matching the condition");
            return new List<ServerResponse>();
        }

        var serverIds = new HashSet<string>(servers.Select(server => server.Id),  StringComparer.OrdinalIgnoreCase);
        var serverStatuses = await serverStatusesRepository.GetLatestByServerIdsAsync(serverIds, cancellationToken);

        logger.LogDebug("Found {ServerStatusCount} serverStatuses for {ServerIdCount} server IDs",
            serverStatuses.Count, serverIds.Count);

        var serverStatusesByServerId = serverStatuses.ToDictionary(p => p.ServerId, p => p.Adapt<ServerStatusResponse>());

        var serverResponses = servers.Select(server =>
        {
            var serverResponse = server.Adapt<ServerResponse>();
            var hasServerStatus = serverStatusesByServerId.TryGetValue(server.Id, out var lastServerStatus);

            if (!hasServerStatus)
            {
                logger.LogDebug("No serverStatus found for server {ServerId}", server.Id);
            }

            return serverResponse with
            {
                LatestStatus = lastServerStatus
            };
        }).ToList();

        var serversWithServerStatus = serverResponses.Count(s => s.LatestStatus != null);

        logger.LogInformation("Found {ServerCount} servers by condition ({WithServerStatusCount} with serverStatus, {WithoutServerStatusCount} without serverStatus)",
            serverResponses.Count, serversWithServerStatus, serverResponses.Count - serversWithServerStatus);

        return serverResponses;
    }

    public async Task<ServerDetailedResponse?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving detailed server information for ID: {ServerId}", id);

        Server server = await serversRepository.GetByIdAsync(id, cancellationToken) ?? throw new ServerNotFoundException(id);

        logger.LogDebug("Server {ServerId} found: {ServerName} ({AppName})", id, server.HostName, server.AppName);

        var serverStatusQueryParameters = new ServerStatusQueryParameters(WithServerId: id, OrderBy: $"{nameof(ServerStatusResponse.RecordedAt)} desc", PageSize: 10);

        var serverStatusesTask = serverStatusesRepository.GetPagedListByQueryAsync(new ServerStatusQuery(serverStatusQueryParameters), cancellationToken);
        var connectionsTask = connectionsRepository.FindByConditionAsync(connection => connection.ServerId == id, cancellationToken);

        await Task.WhenAll(serverStatusesTask, connectionsTask);

        PagedList<ServerStatus> serverStatuses = await serverStatusesTask;
        List<Connection> connections = await connectionsTask;

        logger.LogDebug("Retrieved associated data for server {ServerId}: {ServerStatusCount} serverStatuses, {ConnectionCount} connections", id, serverStatuses.Count, connections.Count);

        var serverResponse = server.Adapt<ServerDetailedResponse>();

        logger.LogInformation("Successfully retrieved detailed server {ServerId} with {ConnectionCount} connections and {ServerStatusCount} recent serverStatuses", id, connections.Count, serverStatuses.Count);

        return serverResponse with
        {
            Connections = connections.Adapt<IList<ConnectionResponse>>(),
            Statuses = serverStatuses.Adapt<IList<ServerStatusResponse>>()
        };
    }

    public async Task<ServerDetailedResponse?> GetByHostAndAppNameAsync(string hostName, string appName, CancellationToken cancellationToken)
    {
        return await GetByIdAsync(IdBuilder.ServerIdFromHostAndApp(hostName, appName), cancellationToken);
    }

    public async Task<ServerResponse> CreateAsync(ServerCreateRequest serverRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new server with HostName: {HostName} AppName: {AppName}", serverRequest.HostName, serverRequest.AppName);

        await hostsService.EnsureHostExistsAsync(serverRequest.HostName, cancellationToken);

        var server = serverRequest.Adapt<Server>() with
        {
            Id = IdBuilder.ServerIdFromHostAndApp(serverRequest.HostName, serverRequest.AppName)
        };

        await serversRepository.CreateAsync(server, cancellationToken);

        logger.LogInformation("Successfully created server with ID: {ServerId}", server.Id);

        return server.Adapt<ServerResponse>();
    }

    public async Task UpdateAsync(string id, ServerUpdateRequest serverRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating server with ID: {ServerId}", id);

        var server = await serversRepository.GetByIdAsync(id, cancellationToken) ?? throw new ServerNotFoundException(id);

        serverRequest.Adapt(server);

        logger.LogDebug("Applying updates to server {ServerId}: HostName={NewName}, AppName={NewAppName}", id, serverRequest.HostName, serverRequest.AppName);
        await serversRepository.UpdateAsync(server, cancellationToken);

        logger.LogInformation("Successfully updated server with ID: {ServerId}", id);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting server with ID: {ServerId}", id);

        var server = await serversRepository.GetByIdAsync(id, cancellationToken) ?? throw new ServerNotFoundException(id);

        await serversRepository.DeleteAsync(server, cancellationToken);

        logger.LogInformation("Successfully deleted server with ID: {ServerId}", id);
    }

    private static Expression<Func<Server, bool>> ConvertExpressionToServerType(Expression<Func<ServerResponse, bool>> expression)
    {
        var parameter = Expression.Parameter(typeof(Server), "server");

        var visitor = new ParameterReplacerVisitor(expression.Parameters[0], parameter);
        var body = visitor.Visit(expression.Body);

        return Expression.Lambda<Func<Server, bool>>(body, parameter);
    }

    private class ParameterReplacerVisitor(ParameterExpression oldParameter, ParameterExpression newParameter) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == oldParameter ? newParameter : base.VisitParameter(node);
        }
    }
}
