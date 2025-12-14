using System.Text.Json;
using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using Application.UseCases.Servers.Create;
using Application.UseCases.Servers.Delete;
using Application.UseCases.Servers.GetById;
using Application.UseCases.Servers.GetByQuery;
using Application.UseCases.Servers.Update;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Watchtower.WebApi.Models;
using Watchtower.WebApi.Utilities;

namespace Watchtower.WebApi.Endpoints;
public static class ServersEndpoints
{
    public static void MapServersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/servers")
            .WithTags("Servers");

        group.MapGet("/", GetByQueryParameters)
            .Produces<IList<ServerResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", GetServerById)
            .Produces<ServerDetailedResponse>(StatusCodes.Status200OK)
            .WithName(nameof(GetServerById));

        group.MapPost("/", CreateServer)
            .Produces<ApiErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ServerDetailedResponse>(StatusCodes.Status201Created);

        group.MapDelete("/{id}", DeleteServer)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id}", UpdateServer)
            .Produces<ApiErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/live", async (
                HttpContext ctx,
                IEventStreamingService<ServerResponse> service,
                SseStreamer streamer,
                CancellationToken cancellationToken) =>
            {
                await streamer.StreamEventsAsync(ctx, service, cancellationToken);
            })
            .WithName("ServerEvents")
            .WithSummary("Stream server events");
    }

    // GET /api/servers
    private static  async Task<IResult> GetByQueryParameters(IQueryHandler<GetServerQuery, PagedList<ServerResponse>> handler, [AsParameters] ServerQueryParameters queryParameters, HttpResponse response, CancellationToken cancellationToken)
    {
        var serversResponse = await handler.HandleAsync(new GetServerQuery(queryParameters), cancellationToken);

        response.Headers.Append("X-Pagination", JsonSerializer.Serialize(serversResponse.MetaData));

        return Results.Ok(serversResponse);
    }

    // GET /api/servers/{id}
    private static async Task<IResult> GetServerById(IQueryHandler<GetServerByIdQuery, ServerDetailedResponse?> handler, string id, CancellationToken cancellationToken)
    {
        var serverResponse = await handler.HandleAsync(new GetServerByIdQuery(id), cancellationToken);
        return Results.Ok(serverResponse);
    }

    // POST /api/servers
    private static async Task<IResult> CreateServer(ICommandHandler<CreateServerCommand, ServerResponse> handler, ServerCreateRequest serverRequest, CancellationToken cancellationToken)
    {
        var serverResponse = await handler.HandleAsync(new CreateServerCommand(serverRequest), cancellationToken);
        return Results.CreatedAtRoute(nameof(GetServerById), new { id = serverResponse.Id }, serverResponse);
    }

    // DELETE /api/servers/{id}
    private static async Task<IResult> DeleteServer(ICommandHandler<DeleteServerCommand> handler, string id, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new DeleteServerCommand(id), cancellationToken);
        return Results.NoContent();
    }

    // PUT /api/servers/{id}
    private static async Task<IResult> UpdateServer(ICommandHandler<UpdateServerCommand> handler, string id, ServerUpdateRequest serverRequest, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new UpdateServerCommand(id, serverRequest), cancellationToken);
        return Results.NoContent();
    }
}
