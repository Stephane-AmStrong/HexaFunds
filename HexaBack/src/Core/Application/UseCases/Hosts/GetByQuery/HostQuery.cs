#nullable enable

using Domain.Entities;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;

namespace Application.UseCases.Hosts.GetByQuery;

public record HostQuery : BaseQuery<Host>
{
    public HostQuery(HostQueryParameters queryParameters) : base(queryParameters.SearchTerm, queryParameters.OrderBy, queryParameters.Page, queryParameters.PageSize)
    {
        if (!string.IsNullOrWhiteSpace(queryParameters.WithName))
        {
            SetFilterExpression
            (
                host => (string.IsNullOrWhiteSpace(queryParameters.WithName) || host.Name == queryParameters.WithName)
            );
        }
    }
}
