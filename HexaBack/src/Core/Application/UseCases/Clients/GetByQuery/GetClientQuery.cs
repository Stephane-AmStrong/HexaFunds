#nullable enable
using Application.Abstractions.Handlers;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Clients.GetByQuery;

public record GetClientQuery(ClientQueryParameters Parameters) : IQuery<PagedList<ClientResponse>>;
