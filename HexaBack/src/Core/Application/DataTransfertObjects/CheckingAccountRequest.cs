namespace Application.DataTransfertObjects;

public record CheckingAccountRequest : BankAccountRequest
{
    public required float OverdraftLimit { get; set; }
}
