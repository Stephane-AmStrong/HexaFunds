namespace DataTransfertObjects;

public record CheckingAccountRequest : BankAccountRequest
{
    public float OverdraftLimit { get; init; }
}
