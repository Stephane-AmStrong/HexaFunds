using Application.Abstractions.Handlers;
using Application.Abstractions.Services;

namespace Application.UseCases.Alerts.Delete;

public class DeleteAlertCommandHandler(IAlertsService alertsService) : ICommandHandler<DeleteAlertCommand>
{
    public Task HandleAsync(DeleteAlertCommand command, CancellationToken cancellationToken)
    {
        return alertsService.DeleteAsync(command.Id, cancellationToken);
    }
}
