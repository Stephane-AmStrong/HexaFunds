using Application.UseCases.CheckingAccounts.Create;

namespace Application.UseCases.CheckingAccounts.GetByQuery;

public record CheckingAccountResponse : CheckingAccountCreateRequest
{
    public Guid Id { get; init; }
    public new float Balance { get; init; }
}
