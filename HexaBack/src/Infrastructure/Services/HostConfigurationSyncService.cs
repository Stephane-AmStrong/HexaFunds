using Application.Abstractions.Services;
using Application.Models.FlattenedConfiguration;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using Microsoft.Extensions.Logging;

namespace Services;

public class HostConfigurationSyncService(
    IFlatConfigurationService flatConfigurationService,
    IHostsRepository hostsRepository,
    ILogger<HostConfigurationSyncService> logger) : IHostConfigurationSyncService
{
    public async Task<(long updateCount, long insertCount)> SyncHostsFromFlatConfigAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Starting host synchronization from flat configuration");

            var (hostConfigs, existingHosts) = await LoadDataConcurrentlyAsync(cancellationToken);

            var (hostsToUpdate, hostsToInsert) = CategorizeHostOperations(hostConfigs, existingHosts);

            return await ExecuteOperationsInParallelAsync(hostsToUpdate, hostsToInsert, cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Critical error during host synchronization");
            return (0,0);
        }
    }

    private async Task<(List<ServerConfig> configs, List<Host> hosts)> LoadDataConcurrentlyAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Loading server configurations and existing hosts concurrently");

        var configsTask = flatConfigurationService.FindServerConfigsByIdentifiersAsync();
        var hostsTask = hostsRepository.FindByConditionAsync(_ => true, cancellationToken);

        await Task.WhenAll(configsTask, hostsTask);
        return ((await configsTask).Where(HasValidId).DistinctBy(cfg => cfg.HostName).ToList(), await hostsTask);
    }

    private (HashSet<Host> toUpdate, HashSet<Host> toInsert) CategorizeHostOperations(
        IList<ServerConfig> serverConfigs,
        List<Host> existingHosts)
    {
        logger.LogDebug("Categorizing {ConfigCount} server configurations for sync operations", serverConfigs.Count());

        // Create a dictionary for faster lookups
        var existingHostsByKey = existingHosts
            .Where(HasValidName)
            .ToDictionary(host => host.Name, StringComparer.OrdinalIgnoreCase);

        // Log any hosts with invalid Names for further investigation
        if (existingHosts.Count != existingHostsByKey.Count)
        {
            var invalidHosts = existingHosts.Where(host => !HasValidName(host)).ToList();
            logger.LogWarning("Found {Count} hosts with invalid Name: {HostIds}", invalidHosts.Count, string.Join(", ", invalidHosts.Select(s => s.Id ?? "<null>")));
        }

        var hostsToUpdate = new HashSet<Host>();
        var hostsToInsert = new HashSet<Host>();

        foreach (var config in serverConfigs)
        {
            var hostCreateFromConfig = new HostCreateRequest{ Name = config.HostName };
            var hostFromConfig = hostCreateFromConfig.Adapt<Host>();

            if (existingHostsByKey.TryGetValue(config.HostName, out var existingHost))
            {
                hostsToUpdate.Add(hostFromConfig with{ Id = existingHost.Id});
            }
            else
            {
                hostsToInsert.Add(hostFromConfig);
            }
        }

        logger.LogInformation("Hosts to update: {UpdateCount}, hosts to insert: {InsertCount}", hostsToUpdate.Count, hostsToInsert.Count);

        return (hostsToUpdate, hostsToInsert);
    }

    private async Task<(long updateCount, long insertCount)> ExecuteOperationsInParallelAsync(
        HashSet<Host> hostsToUpdate,
        HashSet<Host> hostsToInsert,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Executing update and insert operations in parallel");

        // Execute both operations concurrently for maximum performance
        var updateTask = ExecuteBulkUpdateAsync(hostsToUpdate, cancellationToken);
        var insertTask = ExecuteBulkInsertAsync(hostsToInsert, cancellationToken);

        var results = await Task.WhenAll(updateTask, insertTask);
        var totalProcessed = results.Sum();

        LogSyncResults(results[0], results[1], totalProcessed);
        return (results[0], results[1]);
    }

    private async Task<long> ExecuteBulkUpdateAsync(HashSet<Host> hostsToUpdate, CancellationToken cancellationToken)
    {
        if (hostsToUpdate.Count == 0) return 0;

        try
        {
            var updateIds = hostsToUpdate.Select(s => s.Id).ToHashSet();
            await hostsRepository.BulkUpdateAsync(updateIds, hostsToUpdate, cancellationToken);

            logger.LogInformation("Successfully updated {Count} existing hosts", hostsToUpdate.Count);
            return hostsToUpdate.Count;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update {Count} hosts", hostsToUpdate.Count);
            return 0;
        }
    }

    private async Task<long> ExecuteBulkInsertAsync(HashSet<Host> hostsToInsert, CancellationToken cancellationToken)
    {
        if (hostsToInsert.Count == 0) return 0;

        try
        {
            await hostsRepository.BulkInsertAsync(hostsToInsert, cancellationToken);

            logger.LogInformation("Successfully inserted {Count} new hosts", hostsToInsert.Count);
            return hostsToInsert.Count;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to insert {Count} hosts", hostsToInsert.Count);
            return 0;
        }
    }

    private void LogSyncResults(long updateCount, long insertCount, long totalProcessed)
    {
        if (totalProcessed == 0)
        {
            logger.LogInformation("No changes detected. All hosts are up to date");
        }
        else
        {
            logger.LogInformation("Host synchronization completed: {UpdateCount} updated, {InsertCount} inserted, {TotalCount} total processed",
                updateCount, insertCount, totalProcessed);
        }
    }

    // Optimized helper methods
    private static bool HasValidId(ServerConfig config) => !string.IsNullOrEmpty(config.Id) && !string.IsNullOrWhiteSpace(config.HostName);
    private static bool HasValidName(Host host) => !string.IsNullOrEmpty(host.Name);
}
