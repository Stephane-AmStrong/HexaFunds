#nullable enable
using Application.Abstractions.Handlers;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Connections.GetByQuery;

public record GetConnectionQuery(ConnectionQueryParameters Parameters) : IQuery<PagedList<ConnectionResponse>>;
