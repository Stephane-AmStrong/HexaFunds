#nullable enable
using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Alerts.GetByQuery;

public class GetAlertQueryHandler(IAlertsService alertsService) : IQueryHandler<GetAlertQuery, PagedList<AlertResponse>>
{
    public Task<PagedList<AlertResponse>> HandleAsync(GetAlertQuery query, CancellationToken cancellationToken)
    {
        return alertsService.GetPagedListByQueryAsync(new AlertQuery(query.Parameters), cancellationToken);
    }
}
