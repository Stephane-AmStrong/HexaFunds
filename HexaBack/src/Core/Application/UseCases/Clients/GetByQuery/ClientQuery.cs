#nullable enable
using Domain.Entities;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;

namespace Application.UseCases.Clients.GetByQuery;

public record ClientQuery : BaseQuery<Client>
{
    public ClientQuery(ClientQueryParameters queryParameters) : base(queryParameters.SearchTerm, queryParameters.OrderBy, queryParameters.Page, queryParameters.PageSize)
    {
        if (!string.IsNullOrWhiteSpace(queryParameters.WithGaia) || !string.IsNullOrWhiteSpace(queryParameters.WithLogin))
        {
            SetFilterExpression
            (
                client => (string.IsNullOrWhiteSpace(queryParameters.WithGaia) || client.Gaia == queryParameters.WithGaia) && (string.IsNullOrWhiteSpace(queryParameters.WithLogin) || client.Login == queryParameters.WithLogin)
            );
        }
    }
}