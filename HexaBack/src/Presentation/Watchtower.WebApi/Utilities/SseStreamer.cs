using System.Text.Json;
using Application.Abstractions.Services;
using Application.Models;
using MCS.WatchTower.WebApi.DataTransferObjects;
using Microsoft.Net.Http.Headers;

namespace Watchtower.WebApi.Utilities;

public class SseStreamer(SseStreamingOptions options, ILogger<SseStreamer> logger)
{
    public async Task StreamEventsAsync<TDto>(
        HttpContext httpContext,
        IEventStreamingService<TDto> service,
        CancellationToken cancellationToken) where TDto : IBaseDto
    {
        var clientId = httpContext.Connection.Id;
        logger.LogInformation("Starting SSE stream for client {ClientId}", clientId);

        SetResponseHeaders(httpContext);

        try
        {
            while (await service.Events.WaitToReadAsync(cancellationToken))
            {
                var broadcastMessage = await service.Events.ReadAsync(cancellationToken);

                logger.LogDebug("Broadcasting event {EventType} to client {ClientId}", broadcastMessage.MessageType, clientId);

                var eventData = new
                {
                    type = broadcastMessage.MessageType,
                    data = broadcastMessage.Record,
                    timestamp = DateTimeOffset.UtcNow
                };

                var jsonEventData = JsonSerializer.Serialize(eventData, options.JsonOptions);

                await httpContext.Response.WriteAsync(jsonEventData, cancellationToken);
                await httpContext.Response.Body.FlushAsync(cancellationToken);

                logger.LogTrace("Event {EventType} successfully sent to client {ClientId}", broadcastMessage.MessageType, clientId);
            }

            logger.LogInformation("SSE stream completed for client {ClientId}", clientId);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("SSE stream cancelled for client {ClientId} - client disconnected", clientId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during SSE streaming for client {ClientId}", clientId);
        }
        finally
        {
            logger.LogDebug("Completing SSE response for client {ClientId}", clientId);
            await httpContext.Response.CompleteAsync();
        }
    }

    private static void SetResponseHeaders(HttpContext ctx)
    {
        ctx.Response.Headers.Append(HeaderNames.ContentType, "text/event-stream");
        ctx.Response.Headers.Append(HeaderNames.CacheControl, "no-cache");
        ctx.Response.Headers.Append("Connection", "keep-alive");
    }
}
