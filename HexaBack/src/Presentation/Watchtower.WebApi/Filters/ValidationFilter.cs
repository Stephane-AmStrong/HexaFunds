#nullable enable
using FluentValidation;

namespace Watchtower.WebApi.Filters;

public class ValidationFilter<TRequest>(IValidator<TRequest> validator, ILogger<ValidationFilter<TRequest>> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<TRequest>().FirstOrDefault();

        if (request is null)
        {
            logger.LogWarning("Validation failed: Request body is missing");
            return TypedResults.BadRequest(new { Error = "Request body is missing or invalid." });
        }

        var result = await validator.ValidateAsync(request, context.HttpContext.RequestAborted);

        if (!result.IsValid)
        {
            var errors = result.Errors.GroupBy(x => x.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

            logger.LogWarning("Validation failed: {ErrorCount} validation errors. Errors: {FirstError}", 
                result.Errors.Count, 
                string.Join(", ", result.Errors.Select(x => $"[{x.PropertyName}] {x.ErrorMessage}")));

            return TypedResults.ValidationProblem(errors);
        }

        return await next(context);
    }
}