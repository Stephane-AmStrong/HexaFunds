using Application.UseCases.Alerts.GetByQuery;
using Application.UseCases.Connections.GetByQuery;
using Application.UseCases.Servers.GetByQuery;

namespace Application.UseCases.Alerts.GetById;

public record AlertDetailedResponse : AlertResponse
{
    public ServerResponse Server { get; init; }
}
