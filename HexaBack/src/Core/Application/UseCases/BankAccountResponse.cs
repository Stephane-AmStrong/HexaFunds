namespace Application.UseCases;

public record BankAccountResponse : BankAccountRequest
{
    public Guid Id { get; init; }
}
