using Application.Abstractions.Handlers;

namespace Application.UseCases.CheckingAccounts.Update;

public record UpdateCheckingAccountCommand(Guid Id, CheckingAccountUpdateRequest Payload) : ICommand;
