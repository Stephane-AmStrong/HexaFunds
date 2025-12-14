using Application.UseCases.Alerts.Create;

namespace Application.UseCases.Alerts.GetByQuery;

public record AlertResponse : AlertCreateRequest
{
    public Guid Id { get; init; }
}
