using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Servers.GetById;

public record GetServerByIdQuery(string Id) : IQuery<ServerDetailedResponse>;
