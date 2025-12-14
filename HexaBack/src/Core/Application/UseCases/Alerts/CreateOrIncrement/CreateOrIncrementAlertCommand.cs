using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Alerts.CreateOrIncrement;

public record CreateOrIncrementAlertCommand(AlertCreateOrIncrementRequest Payload) : ICommand<AlertResponse>;
