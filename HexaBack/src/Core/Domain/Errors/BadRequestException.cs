#nullable enable
namespace Domain.Errors;

public sealed class BadRequestException(string message) : Exception(message)
{
    public IReadOnlyDictionary<string, string[]>? Errors { get; }

    public BadRequestException(IReadOnlyDictionary<string, string[]> errors) : this("One or more validation errors occurred.")
    {
        Errors = errors;
    }
}
