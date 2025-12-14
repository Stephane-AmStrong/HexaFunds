#nullable enable

using Domain.Entities;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;

namespace Application.UseCases.Servers.GetByQuery;

public record ServerQuery : BaseQuery<Server>
{
    public ServerQuery(ServerQueryParameters queryParameters) : base(queryParameters.SearchTerm, queryParameters.OrderBy, queryParameters.Page, queryParameters.PageSize)
    {
        if (!string.IsNullOrWhiteSpace(queryParameters.WithHostName) || !string.IsNullOrWhiteSpace(queryParameters.WithAppName) || !string.IsNullOrWhiteSpace(queryParameters.WithVersion))
        {
            SetFilterExpression
            (
                server => (string.IsNullOrWhiteSpace(queryParameters.WithHostName) || server.HostName == queryParameters.WithHostName) && (string.IsNullOrWhiteSpace(queryParameters.WithAppName) || server.AppName == queryParameters.WithAppName) && (string.IsNullOrWhiteSpace(queryParameters.WithVersion) || server.Version == queryParameters.WithVersion)
            );
        }
    }
}
