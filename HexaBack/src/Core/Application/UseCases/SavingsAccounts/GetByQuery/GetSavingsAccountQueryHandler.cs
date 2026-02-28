using Application.Abstractions.Handlers;
using Domain.Shared.Common;
using Application.DataTransfertObjects.Responses;
using Domain.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using Mapster;

namespace Application.UseCases.SavingsAccounts.GetByQuery;

public class GetSavingsAccountQueryHandler(ISavingsAccountRepository savingsAccountRepository, ILogger<GetSavingsAccountQueryHandler> logger) : IQueryHandler<GetSavingsAccountQuery, PagedList<SavingsAccountResponse>>
{
    public async Task<PagedList<SavingsAccountResponse>> HandleAsync(GetSavingsAccountQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting savingsAccounts with query parameters: {@QueryParameters}", query);
        var savingsAccounts = await savingsAccountRepository.GetPagedListByQueryAsync(new SavingsAccountQuery(query.Parameters), cancellationToken);

        var savingsAccountResponses = savingsAccounts.Adapt<List<SavingsAccountResponse>>();

        logger.LogInformation("Retrieved savingsAccounts with meta data: {@MetaData}", savingsAccounts.MetaData);
        return new PagedList<SavingsAccountResponse>(savingsAccountResponses, savingsAccounts.MetaData);
    }
}
