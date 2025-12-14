using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Clients.GetById;

public record GetClientByIdQuery(string Id) : IQuery<ClientDetailedResponse>;
