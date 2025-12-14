using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;

namespace Application.UseCases.Servers.Update;

public record UpdateServerCommand(string Id, ServerUpdateRequest Payload) : ICommand;
