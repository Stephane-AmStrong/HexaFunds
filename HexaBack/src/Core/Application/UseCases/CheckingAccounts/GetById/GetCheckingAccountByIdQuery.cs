using Application.Abstractions.Handlers;

namespace Application.UseCases.CheckingAccounts.GetById;

public record GetCheckingAccountByIdQuery(Guid Id) : IQuery<CheckingAccountDetailedResponse?>;
