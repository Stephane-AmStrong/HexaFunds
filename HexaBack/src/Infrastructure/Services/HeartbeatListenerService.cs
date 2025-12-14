#nullable enable
using System.Collections.Concurrent;
using Application.Abstractions.Services;
using Application.Models;
using MCS.WatchTower.WebApi.DataTransferObjects.Enumerations;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Services;

public sealed class HeartbeatListenerService(
    IServiceScopeFactory scopeFactory,
    ILogger<HeartbeatListenerService> logger) : BackgroundService, IHeartbeatNotificationService
{
    private const int HeartbeatTimeoutSeconds = 60;
    private const int MonitoringInterval = 1;

    private readonly ConcurrentDictionary<string, ServerTimeoutMonitor> _monitoringTasks = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("HeartbeatListenerService starting with {TimeoutSeconds}s timeout", HeartbeatTimeoutSeconds);

        try
        {
            using var scope = scopeFactory.CreateScope();
            var services = new RequiredServices(scope);

            await InitializeServerMonitoringAsync(services, stoppingToken);
            await RunMonitoringLoopAsync(services, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("HeartbeatListenerService was cancelled");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Critical error in HeartbeatListenerService");
        }
        finally
        {
            CleanupAllTasks();
            logger.LogInformation("HeartbeatListenerService stopped");
        }
    }

    public void OnServerCreatedOrUpdated(string serverId)
    {
        if (string.IsNullOrWhiteSpace(serverId))
        {
            logger.LogWarning("Received server creation/update notification with null or empty serverId");
            return;
        }

        if (_monitoringTasks.ContainsKey(serverId))
        {
            logger.LogDebug("Server {ServerId} already being monitored - ignoring creation/update notification", serverId);
            return;
        }

        logger.LogDebug("Server {ServerId} created or updated - starting monitoring", serverId);

        try
        {
            ResetServerTimeout(serverId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to start monitoring for server {ServerId}", serverId);
        }
    }

    public void OnServerStatusReceived(string serverId, ServerStatus status)
    {
        if (status != ServerStatus.Up)
        {
            logger.LogDebug("Ignoring heartbeat for server {ServerId} with status {Status} - only Up status triggers timeout reset", serverId, status);
            return;
        }

        if (string.IsNullOrWhiteSpace(serverId))
        {
            logger.LogWarning("Received heartbeat with null or empty serverId");
            return;
        }

        logger.LogDebug("Heartbeat received for server {ServerId} - resetting timeout", serverId);

        try
        {
            ResetServerTimeout(serverId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to reset timeout for server {ServerId}", serverId);
        }
    }

    private async Task InitializeServerMonitoringAsync(RequiredServices services, CancellationToken cancellationToken)
    {
        try
        {
            var servers = await services.Servers.FindByConditionAsync(_ => true, cancellationToken);
            logger.LogInformation("Initializing heartbeat monitoring for {ServerCount} servers", servers.Count);

            var timeout = TimeSpan.FromSeconds(HeartbeatTimeoutSeconds);
            foreach (var server in servers)
            {
                StartMonitoringServer(server.Id, timeout);
            }

            logger.LogDebug("Created {TaskCount} monitoring tasks", _monitoringTasks.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to initialize server monitoring");
        }
    }

    private async Task RunMonitoringLoopAsync(RequiredServices services, CancellationToken cancellationToken)
    {
        logger.LogDebug("Starting timeout monitoring loop");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await ProcessCompletedTimeoutsAsync(services, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in monitoring loop");
                await Task.Delay(TimeSpan.FromSeconds(MonitoringInterval), cancellationToken);
            }
        }

        logger.LogDebug("Timeout monitoring loop stopped");
    }

    private async Task ProcessCompletedTimeoutsAsync(RequiredServices services, CancellationToken cancellationToken)
    {
        if (!_monitoringTasks.Any())
        {
            await Task.Delay(TimeSpan.FromSeconds(MonitoringInterval), cancellationToken);
            return;
        }

        var activeTasks = _monitoringTasks.Values.Select(m => m.TimeoutTask).ToArray();
        var completedTask = await Task.WhenAny(activeTasks);

        var completedMonitor = _monitoringTasks.Values.FirstOrDefault(m => m.TimeoutTask == completedTask);
        if (completedMonitor == null)
        {
            logger.LogDebug("Completed timeout task not found in monitoring collection");
            return;
        }

        try
        {
            await completedTask; // Get the result to check for exceptions
            await HandleServerTimeout(completedMonitor.ServerId, services);
        }
        catch (OperationCanceledException)
        {
            logger.LogTrace("Monitoring task cancelled for server {ServerId} - heartbeat received", completedMonitor.ServerId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing timeout for server {ServerId}", completedMonitor.ServerId);

            if (_monitoringTasks.TryRemove(completedMonitor.ServerId, out var failedMonitor))
            {
                failedMonitor.Dispose();
            }
        }
    }

    private async Task HandleServerTimeout(string serverId, RequiredServices services)
    {
        if (_monitoringTasks.TryRemove(serverId, out var monitor))
        {
            monitor.Dispose();

            try
            {
                await MarkServerAsDownAsync(serverId, services);
                logger.LogWarning("Server {ServerId} timed out and marked as Down", serverId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to mark server {ServerId} as Down after timeout", serverId);
            }
        }
        else
        {
            logger.LogWarning("Timeout occurred for server {ServerId} but monitor not found", serverId);
        }
    }

    private void StartMonitoringServer(string serverId, TimeSpan timeout)
    {
        if (string.IsNullOrWhiteSpace(serverId))
        {
            logger.LogWarning("Cannot start monitoring - serverId is null or empty");
            return;
        }

        var startTime = DateTimeOffset.UtcNow;
        var expectedEndTime = startTime.Add(timeout);

        var monitor = new ServerTimeoutMonitor(serverId, timeout, logger);
        _monitoringTasks[serverId] = monitor;

        logger.LogInformation("Started monitoring server {ServerId} at {StartTime} - timeout at {ExpectedEndTime}",
            serverId, startTime, expectedEndTime);
    }

    private void ResetServerTimeout(string serverId)
    {
        var resetTime = DateTimeOffset.UtcNow;
        var newTimeout = TimeSpan.FromSeconds(HeartbeatTimeoutSeconds);
        var newExpectedEndTime = resetTime.Add(newTimeout);

        if (!_monitoringTasks.TryGetValue(serverId, out var existingMonitor))
        {
            logger.LogInformation("Server {ServerId} came back online - starting new monitoring (timeout: {NewTimeout})",
                serverId, newTimeout);
            StartMonitoringServer(serverId, newTimeout);
            return;
        }

        var oldExpectedEndTime = existingMonitor.StartTime.Add(existingMonitor.Timeout);

        // Cancel and dispose existing monitor
        existingMonitor.Cancel();
        existingMonitor.Dispose();

        // Start new monitoring
        StartMonitoringServer(serverId, newTimeout);

        logger.LogInformation("Reset timeout for server {ServerId} - old end: {OldEnd}, new end: {NewEnd}",
            serverId, oldExpectedEndTime, newExpectedEndTime);
    }

    private async Task MarkServerAsDownAsync(string serverId, RequiredServices services)
    {
        try
        {
            var request = new ServerStatusCreateRequest
            {
                ServerId = serverId,
                Status = ServerStatus.Down
            };

            await services.ServerStatuses.CreateAsync(request, CancellationToken.None);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create Down status for server {ServerId}", serverId);
        }
    }

    private void CleanupAllTasks()
    {
        logger.LogDebug("Cleaning up {TaskCount} monitoring tasks", _monitoringTasks.Count);

        foreach (var monitor in _monitoringTasks.Values)
        {
            try
            {
                monitor.Cancel();
                monitor.Dispose();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Error disposing monitor for server {ServerId}", monitor.ServerId);
            }
        }

        _monitoringTasks.Clear();
        logger.LogDebug("All monitoring tasks cleaned up");
    }

    private record RequiredServices(IServiceScope Scope)
    {
        public IServersService Servers { get; } = Scope.ServiceProvider.GetRequiredService<IServersService>();
        public IServerStatusesService ServerStatuses { get; } = Scope.ServiceProvider.GetRequiredService<IServerStatusesService>();
    }
}
