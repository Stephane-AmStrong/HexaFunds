namespace DataTransfertObjects;

public record TransactionResponse : TransactionRequest
{
    public Guid Id { get; init; }
    public new DateTime Date { get; init; }
}