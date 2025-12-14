#nullable enable
using Domain.Entities;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;

namespace Application.UseCases.ServerStatuses.GetByQuery;

public record ServerStatusQuery : BaseQuery<ServerStatus>
{
    public ServerStatusQuery(ServerStatusQueryParameters queryParameters) : base(queryParameters.SearchTerm, queryParameters.OrderBy, queryParameters.Page, queryParameters.PageSize)
    {
        if (!string.IsNullOrWhiteSpace(queryParameters.WithServerId) || queryParameters.OfStatus is not null || queryParameters.RecordedBefore is not null || queryParameters.RecordedAfter is not null)
        {
            SetFilterExpression
            (
                serverStatus => (string.IsNullOrWhiteSpace(queryParameters.WithServerId) || serverStatus.ServerId == queryParameters.WithServerId) &&
                        (queryParameters.OfStatus == null || serverStatus.Status == queryParameters.OfStatus.ToString()) &&
                        (queryParameters.RecordedBefore == null || serverStatus.RecordedAt < queryParameters.RecordedBefore) &&
                        (queryParameters.RecordedAfter == null || serverStatus.RecordedAt >= queryParameters.RecordedAfter)
            );
        }
    }
}