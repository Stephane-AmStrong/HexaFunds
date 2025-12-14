namespace Application.UseCases.CheckingAccounts.Create;

public record CheckingAccountCreateRequest : BankAccountRequest
{
    public required float OverdraftLimit { get; set; }
}
