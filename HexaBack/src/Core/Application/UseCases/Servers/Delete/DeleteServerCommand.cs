using Application.Abstractions.Handlers;

namespace Application.UseCases.Servers.Delete;

public record DeleteServerCommand(string Id) : ICommand;
