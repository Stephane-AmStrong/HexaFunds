using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Requests;
using Application.DataTransfertObjects.Responses;

namespace Application.UseCases.SavingsAccounts.Create;

public record CreateSavingsAccountCommand(SavingsAccountRequest Payload) : ICommand<SavingsAccountResponse>;
