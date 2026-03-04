using System.Text.Json;
using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.QueryParameters;
using Application.DataTransfertObjects.Requests;
using Application.DataTransfertObjects.Responses;
using Application.UseCases.SavingsAccounts.Create;
using Application.UseCases.SavingsAccounts.Delete;
using Application.UseCases.SavingsAccounts.GetById;
using Application.UseCases.SavingsAccounts.GetByQuery;
using Application.UseCases.SavingsAccounts.Update;
using Domain.Shared.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace HexaFunds.WebApi.Endpoinds;

public static class SavingsAccountsEndpoints
{

    public static void MapSavingsAccountsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/savingsAccounts");

        group.MapGet("/", GetSavingsAccountsByQuery)
            .Produces<IList<SavingsAccountResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", GetSavingsAccountById)
            .Produces<SavingsAccountResponse>(StatusCodes.Status200OK)
            .WithName(nameof(GetSavingsAccountById));

        group.MapPost("/", CreateSavingsAccount)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<SavingsAccountResponse>(StatusCodes.Status201Created);

        group.MapDelete("/{id:guid}", DeleteSavingsAccount)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:guid}", UpdateSavingsAccount)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }

    // GET /api/savingsAccounts
    private static async Task<IResult> GetSavingsAccountsByQuery(IQueryHandler<GetSavingsAccountQuery, PagedList<SavingsAccountResponse>> handler, [AsParameters] SavingsAccountQueryParameters queryParameters, HttpResponse response, IOptions<JsonOptions> jsonOptions, CancellationToken cancellationToken)
    {
        var savingsAccountsResponse = await handler.HandleAsync(new GetSavingsAccountQuery(queryParameters), cancellationToken);

        response.Headers.Append("X-Pagination", JsonSerializer.Serialize(savingsAccountsResponse.MetaData, jsonOptions.Value.SerializerOptions));

        return Results.Ok(savingsAccountsResponse);
    }

    // GET /api/savingsAccounts/{id:guid}
    private static async Task<IResult> GetSavingsAccountById(IQueryHandler<GetSavingsAccountByIdQuery, SavingsAccountResponse?> handler, Guid id, CancellationToken cancellationToken)
    {
        var savingsAccountResponse = await handler.HandleAsync(new GetSavingsAccountByIdQuery(id), cancellationToken);
        return Results.Ok(savingsAccountResponse);
    }

    // POST /api/savingsAccounts
    private static async Task<IResult> CreateSavingsAccount(ICommandHandler<CreateSavingsAccountCommand, SavingsAccountResponse> handler, SavingsAccountRequest savingsAccountRequest, CancellationToken cancellationToken)
    {
        var savingsAccountResponse = await handler.HandleAsync(new CreateSavingsAccountCommand(savingsAccountRequest), cancellationToken);
        return Results.CreatedAtRoute(nameof(GetSavingsAccountById), new { id = savingsAccountResponse.Id }, savingsAccountResponse);
    }

    // DELETE /api/savingsAccounts/{id:guid}
    private static async Task<IResult> DeleteSavingsAccount(ICommandHandler<DeleteSavingsAccountCommand> handler, Guid id, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new DeleteSavingsAccountCommand(id), cancellationToken);
        return Results.NoContent();
    }

    // PUT /api/savingsAccounts/{id:guid}
    private static async Task<IResult> UpdateSavingsAccount(ICommandHandler<UpdateSavingsAccountCommand> handler, Guid id, SavingsAccountRequest savingsAccountRequest, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new UpdateSavingsAccountCommand(id, savingsAccountRequest), cancellationToken);
        return Results.NoContent();
    }
}