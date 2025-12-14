#nullable enable
using Application.UseCases.Hosts.GetByQuery;
using Application.UseCases.Hosts.GetWithServersByQuery;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.Abstractions.Services;

public interface IHostsService
{
    Task<PagedList<HostResponse>> GetPagedListByQueryAsync(HostQuery query, CancellationToken cancellationToken);
    Task<PagedList<HostDetailedResponse>> GetPagedListWithServersByQueryAsync(HostWithServersQuery queryWithServers, CancellationToken cancellationToken);
    Task<HostDetailedResponse?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<HostDetailedResponse?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<HostResponse> CreateAsync(HostCreateRequest hostRequest, CancellationToken cancellationToken);
    Task<HostResponse> EnsureHostExistsAsync(string hostName, CancellationToken cancellationToken);
    Task UpdateAsync(string id, HostUpdateRequest hostRequest, CancellationToken cancellationToken);
    Task DeleteAsync(string id, CancellationToken cancellationToken);
}
