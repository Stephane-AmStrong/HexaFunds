using Application.UseCases.CheckingAccounts.GetByQuery;
using Application.UseCases.Connections.GetByQuery;
using Application.UseCases.Servers.GetByQuery;

namespace Application.UseCases.CheckingAccounts.GetById;

public record CheckingAccountDetailedResponse : CheckingAccountResponse
{
    public ServerResponse? Server { get; init; }
}
