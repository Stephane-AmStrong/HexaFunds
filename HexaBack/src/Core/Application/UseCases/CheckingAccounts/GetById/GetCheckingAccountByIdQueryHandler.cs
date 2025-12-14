using Application.Abstractions.Handlers;
using Application.Abstractions.Services;

namespace Application.UseCases.CheckingAccounts.GetById;

public class GetCheckingAccountByIdQueryHandler(ICheckingAccountsService checkingaccountsService) : IQueryHandler<GetCheckingAccountByIdQuery, CheckingAccountDetailedResponse?>
{
    public Task<CheckingAccountDetailedResponse?> HandleAsync(GetCheckingAccountByIdQuery query, CancellationToken cancellationToken)
    {
        return checkingaccountsService.GetByIdAsync(query.Id, cancellationToken);
    }
}
