using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Requests;

namespace Application.UseCases.SavingsAccounts.Update;

public record UpdateSavingsAccountCommand(Guid Id, SavingsAccountRequest Payload) : ICommand;
