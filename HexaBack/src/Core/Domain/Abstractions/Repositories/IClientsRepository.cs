#nullable enable
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Shared.Common;

namespace Domain.Abstractions.Repositories;

public interface IClientsRepository
{
    Task<PagedList<Client>> GetPagedListByQueryAsync(BaseQuery<Client> queryParameters, CancellationToken cancellationToken);
    Task<List<Client>> FindByConditionAsync(Expression<Func<Client, bool>> expression, CancellationToken cancellationToken);
    Task<Client?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task CreateAsync(Client client, CancellationToken cancellationToken);
    Task UpdateAsync(Client client, CancellationToken cancellationToken);
    Task DeleteAsync(Client client, CancellationToken cancellationToken);
}
