using MCS.WatchTower.WebApi.DataTransferObjects;
using MCS.WatchTower.WebApi.DataTransferObjects.Enumerations;

namespace Application.Models;

public record BroadcastMessage<TDto>(TDto Record, BroadcastMessageType MessageType) where TDto : IBaseDto;
