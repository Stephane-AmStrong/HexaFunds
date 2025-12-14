using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using Application.UseCases.Alerts.GetByQuery;

namespace Application.UseCases.Alerts.Create;

public class CreateAlertCommandHandler(IAlertsService alertsService)
    : ICommandHandler<CreateAlertCommand, AlertResponse>
{
    public Task<AlertResponse> HandleAsync(CreateAlertCommand command, CancellationToken cancellationToken)
    {
        return alertsService.CreateAsync(command.Payload, cancellationToken);
    }
}
