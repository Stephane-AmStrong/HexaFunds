#nullable enable
using Application.Abstractions.Services;
using Application.UseCases.Hosts.GetByQuery;
using Application.UseCases.Hosts.GetWithServersByQuery;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared.Common;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Services;

public sealed class HostsService(IHostsRepository hostsRepository, IServersRepository serversRepository, IServerStatusesRepository serverStatusesRepository, ILogger<HostsService> logger) : IHostsService
{
    public async Task<PagedList<HostResponse>> GetPagedListByQueryAsync(HostQuery queryParameters, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving hosts with query parameters: {@QueryParameters}", queryParameters);
        var hosts = await hostsRepository.GetPagedListByQueryAsync(queryParameters, cancellationToken);

        var hostResponses = hosts.Adapt<List<HostResponse>>();

        logger.LogInformation("Retrieved hosts with meta data: {@MetaData}", hosts.MetaData);
        return new PagedList<HostResponse>(hostResponses, hosts.MetaData);
    }

    public async Task<PagedList<HostDetailedResponse>> GetPagedListWithServersByQueryAsync(HostWithServersQuery queryWithServers, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving hosts with query parameters: {@QueryWithServers}", queryWithServers);
        PagedList<Host> hosts = await hostsRepository.GetPagedListByQueryAsync(queryWithServers, cancellationToken);

        var hostNames = new HashSet<string>(hosts.Select(h => h.Name), StringComparer.OrdinalIgnoreCase);

        List<Server> servers = await serversRepository.FindByConditionAsync(server => hostNames.Contains(server.HostName), cancellationToken);
        logger.LogDebug("Found {ServerCount} servers for {HostNameCount} host names", servers.Count, hosts.Count);


        var serverIds = new HashSet<string>(servers.Select(server => server.Id),  StringComparer.OrdinalIgnoreCase);
        var serverStatuses = await serverStatusesRepository.GetLatestByServerIdsAsync(serverIds, cancellationToken);

        logger.LogDebug("Found {ServerStatusCount} serverStatuses for {ServerIdCount} server IDs", serverStatuses.Count, serverIds.Count);
        var serverStatusesByServerId = serverStatuses.Adapt<List<ServerStatusResponse>>().ToDictionary(p => p.ServerId, p => p);

        var serverResponsesByHostName = servers.Select(server =>
            {
                var serverResponse = server.Adapt<ServerResponse>();
                serverStatusesByServerId.TryGetValue(server.Id, out var lastServerStatus);

                return serverResponse with
                {
                    LatestStatus = lastServerStatus
                };
            }).Adapt<List<ServerResponse>>()
            .GroupBy(server => server.HostName)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

        var hostResponses = hosts.AsParallel().Select(host =>
        {
            var response = host.Adapt<HostDetailedResponse>() with
            {
                Servers = serverResponsesByHostName.TryGetValue(host.Name, out var list) ? list : []
            };
            return response;
        }).ToList();

        logger.LogInformation("Retrieved hosts with meta data: {@MetaData}", hosts.MetaData);
        return new PagedList<HostDetailedResponse>(hostResponses, hosts.MetaData);
    }

    public async Task<HostDetailedResponse?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving host with ID: {HostId}", id);
        var host = await hostsRepository.GetByIdAsync(id, cancellationToken) ?? throw new HostNotFoundException(id);

        List<Server> servers = await serversRepository.FindByConditionAsync(server => server.HostName == host.Name, cancellationToken);
        logger.LogDebug("Found {ServerCount} servers for host {HostId}", servers.Count, id);

        var hostDetailedResponse = host.Adapt<HostDetailedResponse>();

        logger.LogInformation("Successfully retrieved host {HostId} with {ServerCount} servers", id, servers.Count);
        return hostDetailedResponse with
        {
            Servers = servers.Adapt<IList<ServerResponse>>()
        };
    }

    public async Task<HostDetailedResponse?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving host with NAME: {HostName}", name);
        var host = await hostsRepository.GetByNameAsync(name, cancellationToken) ?? throw new HostNotFoundException(name);

        List<Server> servers = await serversRepository.FindByConditionAsync(server => server.HostName == host.Name, cancellationToken);
        logger.LogDebug("Found {ServerCount} servers for host {HostName}", servers.Count, name);

        var hostDetailedResponse = host.Adapt<HostDetailedResponse>();

        logger.LogInformation("Successfully retrieved host {HostName} with {ServerCount} servers", name, servers.Count);
        return hostDetailedResponse with
        {
            Servers = servers.Adapt<IList<ServerResponse>>()
        };
    }

    public async Task<HostResponse> CreateAsync(HostCreateRequest hostRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new host with name: {HostName}", hostRequest.Name);
        var host = hostRequest.Adapt<Host>();

        await hostsRepository.CreateAsync(host, cancellationToken);

        logger.LogInformation("Successfully created host with ID: {HostId}", host.Id);
        return host.Adapt<HostResponse>();
    }

    public async Task<HostResponse> EnsureHostExistsAsync(string hostName, CancellationToken cancellationToken)
    {
        var existingHosts = await hostsRepository.FindByConditionAsync(x => string.Equals(x.Name, hostName, StringComparison.OrdinalIgnoreCase), cancellationToken);

        if (existingHosts.Count != 0)
        {
            var existingHost = existingHosts.First();

            logger.LogInformation("Host with name {HostName} already exists with ID: {HostId}", hostName, existingHost.Id);
            return existingHost.Adapt<HostResponse>();
        }

        logger.LogInformation("Host {HostName} does not exist. Creating host...", hostName);

        var host = new HostCreateRequest { Name = hostName }.Adapt<Host>();

        await hostsRepository.CreateAsync(host, cancellationToken);

        logger.LogInformation("Successfully created host with ID: {HostId}", host.Id);
        return host.Adapt<HostResponse>();
    }

    public async Task UpdateAsync(string id, HostUpdateRequest hostRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating host with ID: {HostId}", id);

        var host = await hostsRepository.GetByIdAsync(id, cancellationToken) ?? throw new HostNotFoundException(id);

        hostRequest.Adapt(host);

        logger.LogDebug("Applying updates to host {HostId}: HostName={NewName}", id, hostRequest.Name);
        await hostsRepository.UpdateAsync(host, cancellationToken);

        logger.LogInformation("Successfully updated host with ID: {HostId}", id);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting host with ID: {HostId}", id);

        var host = await hostsRepository.GetByIdAsync(id, cancellationToken) ?? throw new HostNotFoundException(id);

        await hostsRepository.DeleteAsync(host, cancellationToken);

        logger.LogInformation("Successfully deleted host with ID: {HostId}", id);
    }
}
