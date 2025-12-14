#nullable enable
using Application.UseCases.Clients.GetByQuery;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.Abstractions.Services;

public interface IClientsService
{
    Task<PagedList<ClientResponse>> GetPagedListByQueryAsync(ClientQuery query, CancellationToken cancellationToken);
    Task<ClientDetailedResponse?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<ClientResponse> CreateAsync(ClientCreateRequest clientRequest, CancellationToken cancellationToken);
    Task UpdateAsync(string id, ClientUpdateRequest clientRequest, CancellationToken cancellationToken);
    Task DeleteAsync(string id, CancellationToken cancellationToken);
}
