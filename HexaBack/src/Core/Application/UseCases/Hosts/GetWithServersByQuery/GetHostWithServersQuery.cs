#nullable enable
using Application.Abstractions.Handlers;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Hosts.GetWithServersByQuery;

public record GetHostWithServersQuery(HostQueryParameters Parameters) : IQuery<PagedList<HostDetailedResponse>>;
