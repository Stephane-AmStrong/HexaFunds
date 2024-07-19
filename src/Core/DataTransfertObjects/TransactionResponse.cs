namespace DataTransfertObjects;

public record TransactionResponse : TransactionRequest
{
    public Guid Id { get; init; }
    public DateTime Date { get; init; }
}