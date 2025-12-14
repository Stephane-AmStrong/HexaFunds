namespace Application.Abstractions.Services;

public interface IServerConfigurationSyncService
{
    Task<(long updateCount, long insertCount)> SyncServersFromFlatConfigAsync(CancellationToken cancellationToken);
}
