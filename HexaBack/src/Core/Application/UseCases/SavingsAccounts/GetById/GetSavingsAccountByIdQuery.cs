using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Responses;

namespace Application.UseCases.SavingsAccounts.GetById;

public record GetSavingsAccountByIdQuery(Guid Id) : IQuery<SavingsAccountResponse?>;
