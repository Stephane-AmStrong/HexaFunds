using Application.UseCases.Clients.GetByQuery;
using Application.UseCases.Connections.GetByQuery;

namespace Application.UseCases.Clients.GetById;

public record ClientDetailedResponse : ClientResponse
{
    public IList<ConnectionResponse> Connections { get; init; }
}
