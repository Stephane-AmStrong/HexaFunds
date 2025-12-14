namespace Application.UseCases;

public record SavingsAccountResponse : SavingsAccountRequest
{
    public Guid Id { get; init; }
    public new float Balance { get; init; }
}
