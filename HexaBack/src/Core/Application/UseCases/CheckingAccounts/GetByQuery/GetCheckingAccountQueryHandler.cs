using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Responses;
using Domain.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using Mapster;
using Domain.Shared.Common;

namespace Application.UseCases.CheckingAccounts.GetByQuery;

public class GetCheckingAccountQueryHandler(ICheckingAccountRepository checkingAccountRepository, ILogger<GetCheckingAccountQueryHandler> logger) : IQueryHandler<GetCheckingAccountQuery, PagedList<CheckingAccountResponse>>
{
    public async Task<PagedList<CheckingAccountResponse>> HandleAsync(GetCheckingAccountQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting checkingAccounts with query parameters: {@QueryParameters}", query);
        var checkingAccounts = await checkingAccountRepository.GetPagedListByQueryAsync(new CheckingAccountQuery(query.Parameters), cancellationToken);

        var checkingAccountResponses = checkingAccounts.Adapt<List<CheckingAccountResponse>>();

        logger.LogInformation("Retrieved checkingAccounts with meta data: {@MetaData}", checkingAccounts.MetaData);
        return new PagedList<CheckingAccountResponse>(checkingAccountResponses, checkingAccounts.MetaData);
    }
}
