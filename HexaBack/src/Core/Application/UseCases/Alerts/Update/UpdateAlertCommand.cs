using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;

namespace Application.UseCases.Alerts.Update;

public record UpdateAlertCommand(string Id, AlertUpdateRequest Payload) : ICommand;
