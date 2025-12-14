using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Alerts.GetById;

public record GetAlertByIdQuery(string Id) : IQuery<AlertDetailedResponse>;
