namespace Watchtower.WebApi.Models;

public record ApiErrorResponse(
    string Title,
    int Status,
    IReadOnlyDictionary<string, string[]> Errors,
    string TraceId)
{
    public static ApiErrorResponse ValidationError(IReadOnlyDictionary<string, string[]> errors, string traceId)
        => new(
            "One or more validation errors occurred.",
            StatusCodes.Status400BadRequest,
            errors,
            traceId);

    public static ApiErrorResponse BadRequest(string message, string traceId)
        => new(
            "Bad request.",
            StatusCodes.Status400BadRequest,
            new Dictionary<string, string[]> { ["Request"] = [message] },
            traceId);

    public static ApiErrorResponse NotFound(string message, string traceId)
        => new(
            "Resource not found.",
            StatusCodes.Status404NotFound,
            new Dictionary<string, string[]> { ["Resource"] = [message] },
            traceId);

    public static ApiErrorResponse InvalidJson(string traceId)
        => new(
            "Invalid JSON format.",
            StatusCodes.Status400BadRequest,
            new Dictionary<string, string[]> { ["Json"] = ["Invalid JSON format provided."] },
            traceId);

    public static ApiErrorResponse InternalServerError(string traceId)
        => new(
            "An internal server error occurred.",
            StatusCodes.Status500InternalServerError,
            new Dictionary<string, string[]> { ["Server"] = ["An unexpected error occurred. Please try again later."] },
            traceId);
}
