namespace Application.Abstractions.Services;

public interface IHostConfigurationSyncService
{
    Task<(long updateCount, long insertCount)> SyncHostsFromFlatConfigAsync(CancellationToken cancellationToken);
}
