using Application.DataTransfertObjects.Requests;

namespace Application.DataTransfertObjects.Responses;

public record BankAccountResponse : BankAccountRequest
{
    public Guid Id { get; init; }
}
