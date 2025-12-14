#nullable enable
using Domain.Entities;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;

namespace Application.UseCases.Hosts.GetWithServersByQuery;

public record HostWithServersQuery : BaseQuery<Host>
{
    public HostWithServersQuery(HostQueryParameters queryParameters) : base(queryParameters.SearchTerm, queryParameters.OrderBy, queryParameters.Page, queryParameters.PageSize)
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
