using Application.Abstractions.Handlers;
using Application.UseCases.CheckingAccounts.GetByQuery;

namespace Application.UseCases.CheckingAccounts.Create;

public record CreateCheckingAccountCommand(CheckingAccountCreateRequest Payload) : ICommand<CheckingAccountResponse>;
