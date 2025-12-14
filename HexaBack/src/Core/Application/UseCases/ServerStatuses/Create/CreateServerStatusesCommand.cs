using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.ServerStatuses.Create;

public record CreateServerStatusCommand(ServerStatusCreateRequest Payload) : ICommand<ServerStatusResponse>;
