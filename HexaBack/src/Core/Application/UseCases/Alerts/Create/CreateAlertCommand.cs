using Application.Abstractions.Handlers;
using Application.UseCases.Alerts.GetByQuery;

namespace Application.UseCases.Alerts.Create;

public record CreateAlertCommand(AlertCreateRequest Payload) : ICommand<AlertResponse>;
