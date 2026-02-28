using Application.Abstractions.Handlers;
using Domain.Shared.Common;
using Application.DataTransfertObjects.QueryParameters;
using Application.DataTransfertObjects.Responses;

namespace Application.UseCases.Transactions.GetByQuery;

public record GetTransactionQuery(TransactionQueryParameters Parameters) : IQuery<PagedList<TransactionResponse>>;
