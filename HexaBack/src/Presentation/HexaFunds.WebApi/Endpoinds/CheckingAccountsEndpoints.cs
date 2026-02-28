using System.Text.Json;
using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.QueryParameters;
using Application.DataTransfertObjects.Requests;
using Application.DataTransfertObjects.Responses;
using Application.UseCases.CheckingAccounts.Create;
using Application.UseCases.CheckingAccounts.Delete;
using Application.UseCases.CheckingAccounts.GetById;
using Application.UseCases.CheckingAccounts.GetByQuery;
using Application.UseCases.CheckingAccounts.Update;
using Domain.Shared.Common;
using HexaFunds.WebApi.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace HexaFunds.WebApi.Endpoinds;

public static class CheckingAccountsEndpoints
{

    public static void MapCheckingAccountsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/checkingaccounts");

        group.MapGet("/", GetCheckingAccountsByQuery)
            .Produces<IList<CheckingAccountResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", GetCheckingAccountById)
            .Produces<CheckingAccountResponse>(StatusCodes.Status200OK)
            .WithName(nameof(GetCheckingAccountById));

        group.MapPost("/", CreateCheckingAccount)
            .Produces<ApiErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<CheckingAccountResponse>(StatusCodes.Status201Created);

        group.MapDelete("/{id:guid}", DeleteCheckingAccount)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:guid}", UpdateCheckingAccount)
            .Produces<ApiErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }

    // GET /api/checkingaccounts
    private static async Task<IResult> GetCheckingAccountsByQuery(IQueryHandler<GetCheckingAccountQuery, PagedList<CheckingAccountResponse>> handler, [AsParameters] CheckingAccountQueryParameters queryParameters, HttpResponse response, IOptions<JsonOptions> jsonOptions, CancellationToken cancellationToken)
    {
        var checkingAccountsResponse = await handler.HandleAsync(new GetCheckingAccountQuery(queryParameters), cancellationToken);

        response.Headers.Append("X-Pagination", JsonSerializer.Serialize(checkingAccountsResponse.MetaData, jsonOptions.Value.SerializerOptions));

        return Results.Ok(checkingAccountsResponse);
    }

    // GET /api/checkingaccounts/{id:guid}
    private static async Task<IResult> GetCheckingAccountById(IQueryHandler<GetCheckingAccountByIdQuery, CheckingAccountResponse?> handler, Guid id, CancellationToken cancellationToken)
    {
        var checkingAccountResponse = await handler.HandleAsync(new GetCheckingAccountByIdQuery(id), cancellationToken);
        return Results.Ok(checkingAccountResponse);
    }

    // POST /api/checkingaccounts
    private static async Task<IResult> CreateCheckingAccount(ICommandHandler<CreateCheckingAccountCommand, CheckingAccountResponse> handler, CheckingAccountRequest checkingAccountRequest, CancellationToken cancellationToken)
    {
        var checkingAccountResponse = await handler.HandleAsync(new CreateCheckingAccountCommand(checkingAccountRequest), cancellationToken);
        return Results.CreatedAtRoute(nameof(GetCheckingAccountById), new { id = checkingAccountResponse.Id }, checkingAccountResponse);
    }

    // DELETE /api/checkingaccounts/{id:guid}
    private static async Task<IResult> DeleteCheckingAccount(ICommandHandler<DeleteCheckingAccountCommand> handler, Guid id, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new DeleteCheckingAccountCommand(id), cancellationToken);
        return Results.NoContent();
    }

    // PUT /api/checkingaccounts/{id:guid}
    private static async Task<IResult> UpdateCheckingAccount(ICommandHandler<UpdateCheckingAccountCommand> handler, Guid id, CheckingAccountRequest checkingAccountRequest, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new UpdateCheckingAccountCommand(id, checkingAccountRequest), cancellationToken);
        return Results.NoContent();
    }
}