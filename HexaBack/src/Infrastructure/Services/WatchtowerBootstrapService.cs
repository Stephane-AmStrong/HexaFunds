using Application.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Services;

public class WatchtowerBootstrapService(IServiceProvider serviceProvider, ILogger<WatchtowerBootstrapService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("WatchtowerBootstrapService is starting.");

        using var scope = serviceProvider.CreateScope();

        await BootstrapServerSyncAsync(scope, stoppingToken);

        logger.LogInformation("WatchtowerBootstrapService completed successfully.");
    }

    private async Task BootstrapServerSyncAsync(IServiceScope scope, CancellationToken stoppingToken)
    {
        try
        {
            var serverEnvironmentSyncService = scope.ServiceProvider.GetRequiredService<IServerConfigurationSyncService>();
            var hostEnvironmentSyncService = scope.ServiceProvider.GetRequiredService<IHostConfigurationSyncService>();

            Task<(long updateCount, long insertCount)> serverEnvironmentSyncTask = serverEnvironmentSyncService.SyncServersFromFlatConfigAsync(stoppingToken);
            Task<(long updateCount, long insertCount)> hostEnvironmentSyncTask = hostEnvironmentSyncService.SyncHostsFromFlatConfigAsync(stoppingToken);

            (long updateCount, long insertCount)[] result = await Task.WhenAll(serverEnvironmentSyncTask, hostEnvironmentSyncTask);

            logger.LogInformation(
                "Initial environment sync completed: {ServerUpdateCount} server(s) updated, {ServerInsertCount} server(s) inserted, {HostUpdateCount} host(s) updated, {HostInsertCount} host(s) inserted. Total changes: {TotalChanges}",
                result[0].updateCount, result[0].insertCount, result[1].updateCount, result[1].insertCount, result.Sum(x => x.updateCount + x.insertCount));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to perform initial server sync");
        }
    }
}
