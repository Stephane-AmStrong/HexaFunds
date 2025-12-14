using Application.UseCases.Clients.Update;

namespace Application.UseCases.Clients.Create;

public record ClientCreateRequest : ClientUpdateRequest
{
    public Guid Id { get; init; }
}
