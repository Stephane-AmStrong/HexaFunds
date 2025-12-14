using Application.Abstractions.Handlers;

namespace Application.UseCases.Connections.Terminate;

public record TerminateConnectionCommand(string Id) : ICommand;