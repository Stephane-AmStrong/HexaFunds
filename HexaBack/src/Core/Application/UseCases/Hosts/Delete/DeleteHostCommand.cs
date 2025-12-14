using Application.Abstractions.Handlers;

namespace Application.UseCases.Hosts.Delete;

public record DeleteHostCommand(string Id) : ICommand;
