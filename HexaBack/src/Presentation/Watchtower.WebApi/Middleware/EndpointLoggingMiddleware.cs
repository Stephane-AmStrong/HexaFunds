namespace Watchtower.WebApi.Middleware;

public class EndpointLoggingMiddleware(ILogger<EndpointLoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var method = context.Request.Method;
        var path = context.Request.Path.ToString();
        var endpoint = context.GetEndpoint();
        var displayName = endpoint?.DisplayName ?? path;

        logger.LogInformation(
                "HTTP {Method} {Path} => {EndpointName}",
                method,
                path,
                displayName
            );

        await next(context);
    }
}
