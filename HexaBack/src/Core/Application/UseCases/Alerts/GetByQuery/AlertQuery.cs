#nullable enable
using Domain.Entities;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;

namespace Application.UseCases.Alerts.GetByQuery;

public record AlertQuery : BaseQuery<Alert>
{
    public AlertQuery(AlertQueryParameters queryParameters) : base(queryParameters.SearchTerm, queryParameters.OrderBy, queryParameters.Page, queryParameters.PageSize)
    {
        if (!string.IsNullOrWhiteSpace(queryParameters.WithServerId) || queryParameters.OfType is not null || queryParameters.OfSeverity is not null || queryParameters.OccurredBefore is not null || queryParameters.OccurredAfter is not null)
        {
            SetFilterExpression
            (
                alert => (string.IsNullOrWhiteSpace(queryParameters.WithServerId) || alert.ServerId == queryParameters.WithServerId) &&
                        (queryParameters.OfType == null || alert.Type == queryParameters.OfType.ToString()) &&
                        (queryParameters.OfSeverity == null || alert.Severity == queryParameters.OfSeverity.ToString()) &&
                        (queryParameters.OccurredBefore == null || alert.OccurredAt < queryParameters.OccurredBefore) &&
                        (queryParameters.OccurredAfter == null || alert.OccurredAt >= queryParameters.OccurredAfter)
            );
        }
    }
}
