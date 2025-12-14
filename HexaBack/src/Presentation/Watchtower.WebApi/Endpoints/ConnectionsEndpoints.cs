using System.Text.Json;
using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using Application.UseCases.Connections.Establish;
using Application.UseCases.Connections.GetById;
using Application.UseCases.Connections.GetByQuery;
using Application.UseCases.Connections.Terminate;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Watchtower.WebApi.Models;
using Watchtower.WebApi.Utilities;

namespace Watchtower.WebApi.Endpoints;
public static class ConnectionsEndpoints
{
    public static void MapConnectionsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/connections")
            .WithTags("Connections");

        group.MapGet("/", GetByQueryParameters)
            .Produces<IList<ConnectionResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", GetConnectionById)
            .Produces<ConnectionDetailedResponse>(StatusCodes.Status200OK)
            .WithName(nameof(GetConnectionById));

        group.MapPost("/", EstablishConnection)
            .Produces<ApiErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ConnectionDetailedResponse>(StatusCodes.Status201Created);

        group.MapDelete("/{id}", TerminateConnection)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/live", async (
                HttpContext ctx,
                IEventStreamingService<ConnectionResponse> service,
                SseStreamer streamer,
                CancellationToken cancellationToken) =>
            {
                await streamer.StreamEventsAsync(ctx, service, cancellationToken);
            })
            .WithName("ConnectionEvents")
            .WithSummary("Stream connection events");
    }

    // GET /api/connections
    private static  async Task<IResult> GetByQueryParameters(IQueryHandler<GetConnectionQuery, PagedList<ConnectionResponse>> handler, [AsParameters] ConnectionQueryParameters queryParameters, HttpResponse response, CancellationToken cancellationToken)
    {
        var connectionsResponse = await handler.HandleAsync(new GetConnectionQuery(queryParameters), cancellationToken);

        response.Headers.Append("X-Pagination", JsonSerializer.Serialize(connectionsResponse.MetaData));

        return Results.Ok(connectionsResponse);
    }

    // GET /api/connections/{id}
    private static async Task<IResult> GetConnectionById(IQueryHandler<GetConnectionByIdQuery, ConnectionDetailedResponse?> handler, string id, CancellationToken cancellationToken)
    {
        var connectionResponse = await handler.HandleAsync(new GetConnectionByIdQuery(id), cancellationToken);
        return Results.Ok(connectionResponse);
    }

    // POST /api/connections
    private static async Task<IResult> EstablishConnection(ICommandHandler<EstablishConnectionCommand, ConnectionResponse> handler, ConnectionEstablishRequest connectionRequest, CancellationToken cancellationToken)
    {
        var connectionResponse = await handler.HandleAsync(new EstablishConnectionCommand(connectionRequest), cancellationToken);
        return Results.CreatedAtRoute(nameof(GetConnectionById), new { id = connectionResponse.Id }, connectionResponse);
    }

    // DELETE /api/connections/{id}
    private static async Task<IResult> TerminateConnection(ICommandHandler<TerminateConnectionCommand> handler, string id, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new TerminateConnectionCommand(id), cancellationToken);
        return Results.NoContent();
    }
}
