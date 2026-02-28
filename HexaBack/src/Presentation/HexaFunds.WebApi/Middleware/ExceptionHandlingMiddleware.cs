using System.Text.Json;
using Domain.Exceptions;
using HexaFunds.WebApi.Models;

namespace HexaFunds.WebApi.Middleware;

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
            OperationCanceledException => (
                StatusCodes.Status408RequestTimeout,
                ApiErrorResponse.RequestTimeout(traceId)
            ),
            BadRequestException validationEx => (
                StatusCodes.Status400BadRequest,
                ApiErrorResponse.ValidationError(
                    validationEx.Errors ?? new Dictionary<string, string[]>(),
                    traceId)
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

    private void LogException(Exception e)
    {
        if (e is OperationCanceledException)
        {
            logger.LogDebug("Request canceled or timed out");
        }
        else if (e is BadRequestException validationEx && validationEx.Errors != null)
        {
            logger.LogWarning("Validation error: {Errors}",
                string.Join(", ", validationEx.Errors.SelectMany(err =>
                    err.Value.Select(msg => $"[{err.Key}] {msg}"))));
        }
        else if (e is BadHttpRequestException httpEx)
        {
            logger.LogWarning("Enum binding error: {Message}", httpEx.Message);
        }
        else
        {
            logger.LogError(e, "An unhandled exception occurred: {Message}", e.Message);
        }
    }
}
