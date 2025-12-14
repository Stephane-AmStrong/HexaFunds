using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Hosts.GetServerOfHost;

public record GetServerOfHostQuery(string HostName, string AppName) : IQuery<ServerDetailedResponse?>;
