using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Hosts.GetById;

public record GetHostByIdQuery(string Id) : IQuery<HostDetailedResponse>;
