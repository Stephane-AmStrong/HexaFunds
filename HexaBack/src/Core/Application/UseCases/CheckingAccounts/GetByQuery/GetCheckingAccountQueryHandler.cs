using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using Domain.Shared.Common;

namespace Application.UseCases.CheckingAccounts.GetByQuery;

public class GetCheckingAccountQueryHandler(ICheckingAccountsService checkingaccountsService) : IQueryHandler<GetCheckingAccountQuery, IList<CheckingAccountResponse>>
{
    public Task<IList<CheckingAccountResponse>> HandleAsync(GetCheckingAccountQuery query, CancellationToken cancellationToken)
    {
        // return checkingaccountsService.GetPagedListByQueryAsync(query.Payload, cancellationToken);
        return Task.Run(() => checkingaccountsService.GetAll());
    }
}
