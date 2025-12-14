using Application.Abstractions.Handlers;

namespace Application.UseCases.Clients.Delete;

public record DeleteClientCommand(string Id) : ICommand;
