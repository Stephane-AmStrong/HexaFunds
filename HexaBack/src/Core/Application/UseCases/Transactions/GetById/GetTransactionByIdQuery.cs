using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Responses;

namespace Application.UseCases.Transactions.GetById;

public record GetTransactionByIdQuery(Guid Id) : IQuery<TransactionResponse?>;
