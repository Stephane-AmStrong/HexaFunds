using Application.Abstractions.Handlers;

namespace Application.UseCases.Alerts.Delete;

public record DeleteAlertCommand(string Id) : ICommand;
