using Application.Abstractions.Handlers;
using Domain.Shared.Common;
using Application.DataTransfertObjects.Responses;
using Domain.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using Mapster;

namespace Application.UseCases.Transactions.GetByQuery;

public class GetTransactionQueryHandler(ITransactionRepository transactionRepository, ILogger<GetTransactionQueryHandler> logger) : IQueryHandler<GetTransactionQuery, PagedList<TransactionResponse>>
{
    public async Task<PagedList<TransactionResponse>> HandleAsync(GetTransactionQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting transactions with query parameters: {@QueryParameters}", query);
        var transactions = await transactionRepository.GetPagedListByQueryAsync(new TransactionQuery(query.Parameters), cancellationToken);

        var transactionResponses = transactions.Adapt<List<TransactionResponse>>();

        logger.LogInformation("Retrieved transactions with meta data: {@MetaData}", transactions.MetaData);
        return new PagedList<TransactionResponse>(transactionResponses, transactions.MetaData);
    }
}
