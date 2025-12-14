using System.Text.Json;
using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using Application.UseCases.Clients.Create;
using Application.UseCases.Clients.Delete;
using Application.UseCases.Clients.GetById;
using Application.UseCases.Clients.GetByQuery;
using Application.UseCases.Clients.Update;
using Domain.Shared.Common;
using MCS.WatchTower.WebApi.DataTransferObjects.QueryParameters;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Watchtower.WebApi.Models;
using Watchtower.WebApi.Utilities;

namespace Watchtower.WebApi.Endpoints;
public static class ClientsEndpoints
{
    public static void MapClientsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/clients")
            .WithTags("Clients");

        group.MapGet("/", GetByQueryParameters)
            .Produces<IList<ClientResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", GetClientById)
            .Produces<ClientDetailedResponse>(StatusCodes.Status200OK)
            .WithName(nameof(GetClientById));

        group.MapPost("/", CreateClient)
            .Produces<ApiErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ClientDetailedResponse>(StatusCodes.Status201Created);

        group.MapDelete("/{id}", DeleteClient)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id}", UpdateClient)
            .Produces<ApiErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/live", async (
                HttpContext ctx,
                IEventStreamingService<ClientResponse> service,
                SseStreamer streamer,
                CancellationToken cancellationToken) =>
            {
                await streamer.StreamEventsAsync(ctx, service, cancellationToken);
            })
            .WithName("ClientEvents")
            .WithSummary("Stream client events");
    }

    // GET /api/clients
    private static  async Task<IResult> GetByQueryParameters(IQueryHandler<GetClientQuery, PagedList<ClientResponse>> handler, [AsParameters] ClientQueryParameters queryParameters, HttpResponse response, CancellationToken cancellationToken)
    {
        var clientsResponse = await handler.HandleAsync(new GetClientQuery(queryParameters), cancellationToken);

        response.Headers.Append("X-Pagination", JsonSerializer.Serialize(clientsResponse.MetaData));

        return Results.Ok(clientsResponse);
    }

    // GET /api/clients/{id}
    private static async Task<IResult> GetClientById(IQueryHandler<GetClientByIdQuery, ClientDetailedResponse?> handler, string id, CancellationToken cancellationToken)
    {
        var clientResponse = await handler.HandleAsync(new GetClientByIdQuery(id), cancellationToken);
        return Results.Ok(clientResponse);
    }

    // POST /api/clients
    private static async Task<IResult> CreateClient(ICommandHandler<CreateClientCommand, ClientResponse> handler, ClientCreateRequest clientRequest, CancellationToken cancellationToken)
    {
        var clientResponse = await handler.HandleAsync(new CreateClientCommand(clientRequest), cancellationToken);
        return Results.CreatedAtRoute(nameof(GetClientById), new { id = clientResponse.Id }, clientResponse);
    }

    // DELETE /api/clients/{id}
    private static async Task<IResult> DeleteClient(ICommandHandler<DeleteClientCommand> handler, string id, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new DeleteClientCommand(id), cancellationToken);
        return Results.NoContent();
    }

    // PUT /api/clients/{id}
    private static async Task<IResult> UpdateClient(ICommandHandler<UpdateClientCommand> handler, string id, ClientUpdateRequest clientRequest, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new UpdateClientCommand(id, clientRequest), cancellationToken);
        return Results.NoContent();
    }
}
