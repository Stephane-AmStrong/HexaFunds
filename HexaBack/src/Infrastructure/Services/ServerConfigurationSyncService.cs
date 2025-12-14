using Application.Abstractions.Services;
using Application.Models.FlattenedConfiguration;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Utilities;
using Microsoft.Extensions.Logging;

namespace Services;

public class ServerConfigurationSyncService(
    IFlatConfigurationService flatConfigurationService,
    IServersRepository serversRepository,
    ILogger<ServerConfigurationSyncService> logger) : IServerConfigurationSyncService
{
    public async Task<(long updateCount, long insertCount)> SyncServersFromFlatConfigAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Starting server synchronization from flat configuration");

            var (serverConfigs, existingServers) = await LoadDataConcurrentlyAsync(cancellationToken);

            if (!ValidateDataForSync(serverConfigs)) return (0,0);

            var (serversToUpdate, serversToInsert) = CategorizeServerOperations(serverConfigs, existingServers);

            return await ExecuteOperationsInParallelAsync(serversToUpdate, serversToInsert, cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Critical error during server synchronization");
            return (0, 0);
        }
    }

    private async Task<(IList<ServerConfig> configs, List<Server> servers)> LoadDataConcurrentlyAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Loading server configurations and existing servers concurrently");

        var configsTask = flatConfigurationService.FindServerConfigsByIdentifiersAsync();
        var serversTask = serversRepository.FindByConditionAsync(_ => true, cancellationToken);

        await Task.WhenAll(configsTask, serversTask);
        return (await configsTask, await serversTask);
    }

    private (HashSet<Server> toUpdate, HashSet<Server> toInsert) CategorizeServerOperations(
        IList<ServerConfig> serverConfigs,
        List<Server> existingServers)
    {
        logger.LogDebug("Categorizing {ConfigCount} server configurations for sync operations", serverConfigs.Count);

        // Create a dictionary for faster lookups (config ID → config)
        var existingServersByKey = existingServers
            .Where(HasValidId)
            .ToDictionary(server => server.Id, StringComparer.OrdinalIgnoreCase);

        // Log any servers with invalid IDs for further investigation
        if (existingServers.Count != existingServersByKey.Count)
        {
            var invalidServers = existingServers.Where(s => !HasValidId(s)).ToList();
            logger.LogWarning("Found {Count} servers with invalid HostName & AppNames: {HostNameAppNames}", invalidServers.Count, string.Join(", ", invalidServers.Select(s => IdBuilder.ServerIdFromHostAndApp(s.HostName, s.AppName) ?? "<null>")));
        }

        var serversToUpdate = new HashSet<Server>();
        var serversToInsert = new HashSet<Server>();

        var validConfigs = serverConfigs.Where(HasValidId);

        foreach (var config in validConfigs)
        {
            string computedId = IdBuilder.ServerIdFromHostAndApp(config.HostName, config.AppName);

            var serverFromConfig = config.Adapt<Server>() with
            {
                Id = computedId
            };

            if (existingServersByKey.ContainsKey(computedId))
            {
                serversToUpdate.Add(serverFromConfig);
            }
            else
            {
                serversToInsert.Add(serverFromConfig);
            }
        }

        logger.LogInformation("Servers to update: {UpdateCount}, servers to insert: {InsertCount}",
            serversToUpdate.Count, serversToInsert.Count);

        return (serversToUpdate, serversToInsert);
    }

    private async Task<(long updateCount, long insertCount)> ExecuteOperationsInParallelAsync(
        HashSet<Server> serversToUpdate,
        HashSet<Server> serversToInsert,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Executing update and insert operations in parallel");

        // Execute both operations concurrently for maximum performance
        var updateTask = ExecuteBulkUpdateAsync(serversToUpdate, cancellationToken);
        var insertTask = ExecuteBulkInsertAsync(serversToInsert, cancellationToken);

        var results = await Task.WhenAll(updateTask, insertTask);
        var totalProcessed = results.Sum();

        LogSyncResults(results[0], results[1], totalProcessed);
        return (results[0], results[1]);
    }

    private async Task<long> ExecuteBulkUpdateAsync(HashSet<Server> serversToUpdate, CancellationToken cancellationToken)
    {
        if (serversToUpdate.Count == 0) return 0;

        try
        {
            var updateIds = serversToUpdate.Select(s => s.Id).ToHashSet();
            await serversRepository.BulkUpdateAsync(updateIds, serversToUpdate, cancellationToken);

            logger.LogInformation("Successfully updated {Count} existing servers", serversToUpdate.Count);
            return serversToUpdate.Count;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update {Count} servers", serversToUpdate.Count);
            return 0;
        }
    }

    private async Task<long> ExecuteBulkInsertAsync(HashSet<Server> serversToInsert, CancellationToken cancellationToken)
    {
        if (serversToInsert.Count == 0) return 0;

        try
        {
            await serversRepository.BulkInsertAsync(serversToInsert, cancellationToken);

            logger.LogInformation("Successfully inserted {Count} new servers", serversToInsert.Count);
            return serversToInsert.Count;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to insert {Count} servers", serversToInsert.Count);
            return 0;
        }
    }

    private void LogSyncResults(long updateCount, long insertCount, long totalProcessed)
    {
        if (totalProcessed == 0)
        {
            logger.LogInformation("No changes detected. All servers are up to date");
        }
        else
        {
            logger.LogInformation("Server synchronization completed: {UpdateCount} updated, {InsertCount} inserted, {TotalCount} total processed",
                updateCount, insertCount, totalProcessed);
        }
    }

    // Optimized helper methods
    private static bool ValidateDataForSync(IList<ServerConfig> configs)
    {
        return configs?.Count > 0;
    }

    private static bool HasValidId(ServerConfig config) => !string.IsNullOrEmpty(config.Id);
    private static bool HasValidId(Server server) => !string.IsNullOrEmpty(server.Id);
}
