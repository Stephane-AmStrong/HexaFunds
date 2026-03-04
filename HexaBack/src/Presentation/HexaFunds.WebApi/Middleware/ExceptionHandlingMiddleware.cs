using System.Text.Json;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HexaFunds.WebApi.Middleware;

internal sealed class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, IProblemDetailsService problemDetailsService) : IExceptionHandler
{

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        LogException(exception);
        httpContext.Response.Clear();
        httpContext.Response.ContentType = "application/json";

        var (statusCode, problemDetails) = GetErrorResponse(exception);
        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetails
        });
    }

    private (int StatusCode, ProblemDetails problemDetails) GetErrorResponse(Exception exception)
    {
        return exception switch
        {
            OperationCanceledException => (
                StatusCodes.Status408RequestTimeout,
                new ProblemDetails
                {
                    Title = "The request timed out",
                    Type = StatusCodes.Status408RequestTimeout.ToString(),
                    Detail = "The operation was canceled or timed out"
                }
            ),
            BadRequestException validationEx => (
                StatusCodes.Status400BadRequest,
                new ProblemDetails
                {
                    Title = "One or more validation errors occurred",
                    Type = StatusCodes.Status400BadRequest.ToString(),
                    Detail = string.Join(", ", validationEx?.Errors?.SelectMany(error => error.Value) ?? [])
                }
            ),
            BadHttpRequestException httpEx => (
                StatusCodes.Status400BadRequest,
                new ProblemDetails
                {
                    Title = "Bad request",
                    Type = StatusCodes.Status400BadRequest.ToString(),
                    Detail = httpEx.Message
                }
            ),
            NotFoundException notFoundEx => (
                StatusCodes.Status404NotFound,
                new ProblemDetails
                {
                    Title = "Resource not found",
                    Type = StatusCodes.Status404NotFound.ToString(),
                    Detail = notFoundEx.Message
                }
            ),
            JsonException => (
                StatusCodes.Status400BadRequest,
                new ProblemDetails
                {
                    Title = "Invalid JSON format",
                    Type = StatusCodes.Status400BadRequest.ToString(),
                    Detail = "Invalid JSON format provided"
                }
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "An internal server error occurred",
                    Type = StatusCodes.Status500InternalServerError.ToString(),
                    Detail = "An unexpected error occurred. Please try again later"
                }
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
