using MCS.WatchTower.WebApi.DataTransferObjects.Enumerations;

namespace Application.Abstractions.Services;

public interface IHeartbeatNotificationService
{
    void OnServerStatusReceived(string serverId, ServerStatus status);
    void OnServerCreatedOrUpdated(string serverId);
}
