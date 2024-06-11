namespace DataTransfertObjects;

public record TransactionResponse : TransactionRequest
{
    public Guid Id { get; init; }
    public DateTime Date { get; init; }
    public virtual required BankAccountResponse BankAccount { get; init; }
}
