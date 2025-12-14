#nullable enable
using Domain.Entities;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;

namespace Application.UseCases.Connections.GetByQuery;

public record class ConnectionQuery : BaseQuery<Connection>
{
    public ConnectionQuery(ConnectionQueryParameters queryParameters) : base(queryParameters.SearchTerm, queryParameters.OrderBy, queryParameters.Page, queryParameters.PageSize)
    {
        if (!string.IsNullOrWhiteSpace(queryParameters.WithClientId) || !string.IsNullOrWhiteSpace(queryParameters.WithServerId) || !string.IsNullOrWhiteSpace(queryParameters.WithApplication))
        {
            SetFilterExpression
            (
                connection => (string.IsNullOrWhiteSpace(queryParameters.WithClientId) || connection.ClientId == queryParameters.WithClientId) &&
                              (string.IsNullOrWhiteSpace(queryParameters.WithServerId) || connection.ServerId == queryParameters.WithServerId) &&
                              (string.IsNullOrWhiteSpace(queryParameters.WithApplication) || connection.Application == queryParameters.WithApplication)
            );
        }
    }
}