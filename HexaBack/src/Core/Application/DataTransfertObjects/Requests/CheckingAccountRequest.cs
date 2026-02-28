namespace Application.DataTransfertObjects.Requests;

public record CheckingAccountRequest : BankAccountRequest
{
    public required float OverdraftLimit { get; set; }
}
