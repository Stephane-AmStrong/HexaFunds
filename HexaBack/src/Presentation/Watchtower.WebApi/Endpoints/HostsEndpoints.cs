using System.Text.Json;
using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using Application.UseCases.Hosts.Create;
using Application.UseCases.Hosts.Delete;
using Application.UseCases.Hosts.GetById;
using Application.UseCases.Hosts.GetByName;
using Application.UseCases.Hosts.GetByQuery;
using Application.UseCases.Hosts.GetServerOfHost;
using Application.UseCases.Hosts.GetWithServersByQuery;
using Application.UseCases.Hosts.Update;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Mvc;
using Watchtower.WebApi.Models;
using Watchtower.WebApi.Utilities;

namespace Watchtower.WebApi.Endpoints;
public static class HostsEndpoints
{
    public static void MapHostsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/hosts")
            .WithTags("Hosts");

        group.MapGet("/", GetByQuery)
            .Produces<IList<HostResponse>>(StatusCodes.Status200OK);

        group.MapGet("/with-servers", GetWithServersByQuery)
            .Produces<IList<HostResponse>>(StatusCodes.Status200OK);

        group.MapGet("/by-id/{id}", GetHostById)
            .Produces<HostDetailedResponse>(StatusCodes.Status200OK)
            .WithName(nameof(GetHostById));

        group.MapGet("/{name}", GetHostByName)
            .Produces<HostDetailedResponse>(StatusCodes.Status200OK)
            .WithName(nameof(GetHostByName));

        group.MapGet("/{hostName}/servers/{appName}", GetServerOfHostQuery)
            .Produces<ServerResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName(nameof(GetServerOfHostQuery));

        group.MapPost("/", CreateHost)
            .Produces<ApiErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<HostDetailedResponse>(StatusCodes.Status201Created);

        group.MapDelete("/{id}", DeleteHost)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id}", UpdateHost)
            .Produces<ApiErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/live", async (
                HttpContext ctx,
                IEventStreamingService<HostResponse> service,
                SseStreamer streamer,
                CancellationToken cancellationToken) =>
            {
                await streamer.StreamEventsAsync(ctx, service, cancellationToken);
            })
            .WithName("HostEvents")
            .WithSummary("Stream host events");
    }

    // GET /api/hosts
    private static  async Task<IResult> GetByQuery(IQueryHandler<GetHostQuery, PagedList<HostResponse>> handler, [AsParameters] HostQueryParameters queryParameters, HttpResponse response, CancellationToken cancellationToken)
    {
        var hostsResponse = await handler.HandleAsync(new GetHostQuery(queryParameters), cancellationToken);

        response.Headers.Append("X-Pagination", JsonSerializer.Serialize(hostsResponse.MetaData));

        return Results.Ok(hostsResponse);
    }

    // GET /api/hosts/with-servers
    private static  async Task<IResult> GetWithServersByQuery(IQueryHandler<GetHostWithServersQuery, PagedList<HostDetailedResponse>> handler, [AsParameters] HostQueryParameters queryParameters, HttpResponse response, CancellationToken cancellationToken)
    {
        var hostsResponse = await handler.HandleAsync(new GetHostWithServersQuery(queryParameters), cancellationToken);

        response.Headers.Append("X-Pagination", JsonSerializer.Serialize(hostsResponse.MetaData));

        return Results.Ok(hostsResponse);
    }

    // GET /api/hosts/by-id/{id}
    private static async Task<IResult> GetHostById([FromServices] IQueryHandler<GetHostByIdQuery, HostDetailedResponse?> handler, string id, CancellationToken cancellationToken)
    {
        var hostResponse = await handler.HandleAsync(new GetHostByIdQuery(id), cancellationToken);
        return Results.Ok(hostResponse);
    }

    // GET /api/hosts/{name}
    private static async Task<IResult> GetHostByName(IQueryHandler<GetHostByNameQuery, HostDetailedResponse?> handler, string name, CancellationToken cancellationToken)
    {
        var hostResponse = await handler.HandleAsync(new GetHostByNameQuery(name), cancellationToken);
        return Results.Ok(hostResponse);
    }

    // GET /api/hosts/{name}/servers/{appName}
    private static async Task<IResult> GetServerOfHostQuery([FromServices] IQueryHandler<GetServerOfHostQuery, ServerDetailedResponse?> handler, [FromRoute] string hostName, [FromRoute] string appName, CancellationToken cancellationToken)
    {
        var response = await handler.HandleAsync(new GetServerOfHostQuery(hostName, appName), cancellationToken);
        return response is not null ? Results.Ok(response) : Results.NotFound();
    }

    // POST /api/hosts
    private static async Task<IResult> CreateHost(ICommandHandler<CreateHostCommand, HostResponse> handler, HostCreateRequest hostRequest, CancellationToken cancellationToken)
    {
        var hostResponse = await handler.HandleAsync(new CreateHostCommand(hostRequest), cancellationToken);
        return Results.CreatedAtRoute(nameof(GetHostById), new { id = hostResponse.Id }, hostResponse);
    }

    // DELETE /api/hosts/{id}
    private static async Task<IResult> DeleteHost(ICommandHandler<DeleteHostCommand> handler, string id, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new DeleteHostCommand(id), cancellationToken);
        return Results.NoContent();
    }

    // PUT /api/hosts/{id}
    private static async Task<IResult> UpdateHost(ICommandHandler<UpdateHostCommand> handler, string id, HostUpdateRequest hostRequest, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new UpdateHostCommand(id, hostRequest), cancellationToken);
        return Results.NoContent();
    }
}
