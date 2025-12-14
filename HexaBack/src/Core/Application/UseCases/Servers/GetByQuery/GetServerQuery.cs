#nullable enable
using Application.Abstractions.Handlers;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Servers.GetByQuery;

public record GetServerQuery(ServerQueryParameters Parameters) : IQuery<PagedList<ServerResponse>>;
