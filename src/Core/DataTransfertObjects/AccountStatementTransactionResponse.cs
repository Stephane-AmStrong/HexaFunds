namespace DataTransfertObjects;

public record AccountStatementTransactionResponse : TransactionRequest
{
    public Guid Id { get; init; }
}