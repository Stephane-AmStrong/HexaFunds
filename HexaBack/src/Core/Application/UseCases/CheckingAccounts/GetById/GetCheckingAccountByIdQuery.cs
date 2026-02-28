using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Responses;

namespace Application.UseCases.CheckingAccounts.GetById;

public record GetCheckingAccountByIdQuery(Guid Id) : IQuery<CheckingAccountResponse?>;
