namespace Application.DataTransfertObjects.Requests;

public record SavingsAccountRequest : BankAccountRequest
{
    public required float BalanceCeiling { get; init; }
}
