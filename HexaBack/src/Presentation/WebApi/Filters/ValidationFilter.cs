using System;
using FluentValidation;

namespace WebApi.Filters;

public class ValidationFilter<TRequest>(IValidator<TRequest> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<TRequest>().FirstOrDefault();

        if (request is null) return TypedResults.BadRequest("Request body is missing.");

        var result = await validator.ValidateAsync(request, context.HttpContext.RequestAborted);

        if (!result.IsValid) return TypedResults.BadRequest(result.Errors.Select(error => new { error.PropertyName, error.ErrorMessage }));

        return await next(context);
    }
}
