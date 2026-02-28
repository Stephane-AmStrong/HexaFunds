using Application.DataTransfertObjects.Requests;

namespace Application.DataTransfertObjects.Responses;

public record TransactionResponse : TransactionRequest, IBaseDto
{
    public Guid Id { get; init; }
    public new DateTime Date { get; init; }
}