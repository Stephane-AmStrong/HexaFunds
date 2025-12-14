using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;

namespace Application.UseCases.Hosts.Update;

public record UpdateHostCommand(string Id, HostUpdateRequest Payload) : ICommand;
