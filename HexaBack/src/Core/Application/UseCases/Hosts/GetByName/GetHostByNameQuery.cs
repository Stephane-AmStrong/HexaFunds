using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Hosts.GetByName;

public record GetHostByNameQuery(string Name) : IQuery<HostDetailedResponse>;
