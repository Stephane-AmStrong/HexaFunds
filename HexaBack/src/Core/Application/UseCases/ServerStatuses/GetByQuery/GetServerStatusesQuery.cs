#nullable enable
using Application.Abstractions.Handlers;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.ServerStatuses.GetByQuery;

public record GetServerStatusQuery(ServerStatusQueryParameters Parameters) : IQuery<PagedList<ServerStatusResponse>>;
