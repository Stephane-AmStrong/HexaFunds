namespace HexaFunds.WebApi.Middleware;

public class EndpointLoggingMiddleware(RequestDelegate next, ILogger<EndpointLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
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
