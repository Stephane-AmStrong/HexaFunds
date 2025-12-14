using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Connections.GetById;

public record GetConnectionByIdQuery(string Id) : IQuery<ConnectionDetailedResponse>;
