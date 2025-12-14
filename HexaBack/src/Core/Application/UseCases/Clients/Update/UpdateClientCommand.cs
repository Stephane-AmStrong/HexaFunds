using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;

namespace Application.UseCases.Clients.Update;

public record UpdateClientCommand(string Id, ClientUpdateRequest Payload) : ICommand;
