using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Clients.Create;

public record CreateClientCommand(ClientCreateRequest Payload) : ICommand<ClientResponse>;
