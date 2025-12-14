using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Hosts.Create;

public record CreateHostCommand(HostCreateRequest Payload) : ICommand<HostResponse>;
