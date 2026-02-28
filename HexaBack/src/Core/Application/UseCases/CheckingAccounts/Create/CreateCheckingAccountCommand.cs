using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Requests;
using Application.DataTransfertObjects.Responses;

namespace Application.UseCases.CheckingAccounts.Create;

public record CreateCheckingAccountCommand(CheckingAccountRequest Payload) : ICommand<CheckingAccountResponse>;
