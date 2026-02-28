using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Requests;

namespace Application.UseCases.CheckingAccounts.Update;

public record UpdateCheckingAccountCommand(Guid Id, CheckingAccountRequest Payload) : ICommand;
