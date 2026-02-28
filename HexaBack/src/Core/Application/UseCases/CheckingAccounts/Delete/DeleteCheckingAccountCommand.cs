using Application.Abstractions.Handlers;

namespace Application.UseCases.CheckingAccounts.Delete;

public record DeleteCheckingAccountCommand(Guid Id) : ICommand;
