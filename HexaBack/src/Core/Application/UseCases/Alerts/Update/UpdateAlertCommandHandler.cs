using Application.Abstractions.Handlers;
using Application.Abstractions.Services;

namespace Application.UseCases.Alerts.Update;

public class UpdateAlertCommandHandler(IAlertsService alertsService) : ICommandHandler<UpdateAlertCommand>
{
    public Task HandleAsync(UpdateAlertCommand command, CancellationToken cancellationToken)
    {
        return alertsService.UpdateAsync(command.Id, command.Payload, cancellationToken);
    }
}
