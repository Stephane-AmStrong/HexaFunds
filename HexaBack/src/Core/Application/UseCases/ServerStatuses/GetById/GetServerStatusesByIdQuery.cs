using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.ServerStatuses.GetById;

public record GetServerStatusByIdQuery(string Id) : IQuery<ServerStatusDetailedResponse>;
