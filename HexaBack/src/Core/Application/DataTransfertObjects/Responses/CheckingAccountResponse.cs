using Application.DataTransfertObjects.Requests;

namespace Application.DataTransfertObjects.Responses;

public record CheckingAccountResponse : CheckingAccountRequest, IBaseDto
{
    public Guid Id { get; init; }
    public new float Balance { get; init; }
}
