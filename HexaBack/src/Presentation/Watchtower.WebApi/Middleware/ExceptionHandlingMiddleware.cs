using System.Text.Json;
using Domain.Errors;
using MCS.WatchTower.WebApi.DataTransferObjects.Enumerations;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;
using Watchtower.WebApi.Models;

namespace Watchtower.WebApi.Middleware;

internal sealed class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            LogException(e);
            await HandleExceptionAsync(context, e);
        }
    }
    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        if (httpContext.Response.HasStarted) return;

        httpContext.Response.Clear();
        httpContext.Response.ContentType = "application/json";

        var (statusCode, errorResponse) = GetErrorResponse(exception, httpContext.TraceIdentifier);
        httpContext.Response.StatusCode = statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
    }

    private static (int StatusCode, ApiErrorResponse Response) GetErrorResponse(Exception exception, string traceId)
    {
        return exception switch
        {
            BadRequestException validationEx => (
                StatusCodes.Status400BadRequest,
                ApiErrorResponse.ValidationError(
                    validationEx.Errors ?? new Dictionary<string, string[]>(),
                    traceId)
            ),
            BadHttpRequestException httpEx when IsEnumBindingError(httpEx.Message) => (
                StatusCodes.Status400BadRequest,
                CreateEnumValidationResponse(httpEx.Message, traceId)
            ),
            BadHttpRequestException httpEx => (
                StatusCodes.Status400BadRequest,
                ApiErrorResponse.BadRequest(httpEx.Message, traceId)
            ),
            NotFoundException notFoundEx => (
                StatusCodes.Status404NotFound,
                ApiErrorResponse.NotFound(notFoundEx.Message, traceId)
            ),
            JsonException => (
                StatusCodes.Status400BadRequest,
                ApiErrorResponse.InvalidJson(traceId)
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                ApiErrorResponse.InternalServerError(traceId)
            )
        };
    }

    private static ApiErrorResponse CreateEnumValidationResponse(string errorMessage, string traceId)
    {
        var match = System.Text.RegularExpressions.Regex.Match(
            errorMessage,
            @"Failed to bind parameter "".*?(\w+)"" from ""(.*?)""");

        if (!match.Success)
        {
            return ApiErrorResponse.ValidationError(
                new Dictionary<string, string[]>
                {
                    ["Parameter"] = ["Invalid enum parameter value"]
                },
                traceId);
        }

        var paramName = match.Groups[1].Value;
        var invalidValue = match.Groups[2].Value;
        var errors = new Dictionary<string, string[]>();

        if (EnumParameters.TryGetValue(paramName, out var enumInfo))
        {
            var validValues = string.Join(", ", Enum.GetNames(enumInfo.EnumType));
            errors[paramName] = [$"Invalid {enumInfo.FriendlyName} '{invalidValue}'. Valid values are: {validValues}"];
        }
        else
        {
            errors[paramName] = [$"Invalid enum value '{invalidValue}'"];
        }

        return ApiErrorResponse.ValidationError(errors, traceId);
    }

    private static bool IsEnumBindingError(string errorMessage)
    {
        if (!errorMessage.Contains("Failed to bind parameter")) return false;
        return EnumParameters.Keys.Any(paramName => errorMessage.Contains(paramName));
    }

    private static readonly Dictionary<string, (Type EnumType, string FriendlyName)> EnumParameters =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { nameof(AlertQueryParameters.OfSeverity), (typeof(AlertSeverity), "severity") },
            { nameof(AlertQueryParameters.OfType), (typeof(AlertType), "alert type") }
        };

    private void LogException(Exception e)
    {
        if (e is BadRequestException validationEx && validationEx.Errors != null)
        {
            logger.LogWarning("Validation error: {Errors}",
                string.Join(", ", validationEx.Errors.SelectMany(err =>
                    err.Value.Select(msg => $"[{err.Key}] {msg}"))));
        }
        else if (e is BadHttpRequestException httpEx && IsEnumBindingError(httpEx.Message))
        {
            logger.LogWarning("Enum binding error: {Message}", httpEx.Message);
        }
        else
        {
            logger.LogError(e, "An unhandled exception occurred: {Message}", e.Message);
        }
    }
}
