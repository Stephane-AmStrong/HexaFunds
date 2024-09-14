namespace DataTransfertObjects;

public record CheckingAccountResponse : CheckingAccountRequest
{
    public Guid Id { get; init; }
    public new float Balance { get; init; }
}
