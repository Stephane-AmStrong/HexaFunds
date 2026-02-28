using Application.Abstractions.Handlers;

namespace Application.UseCases.SavingsAccounts.Delete;

public record DeleteSavingsAccountCommand(Guid Id) : ICommand;
