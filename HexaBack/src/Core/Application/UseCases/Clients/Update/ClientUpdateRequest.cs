namespace Application.UseCases.Clients.Update;

public record ClientUpdateRequest
{
    public string Login { get; init; }
    public string Gaia { get; init; }
}
