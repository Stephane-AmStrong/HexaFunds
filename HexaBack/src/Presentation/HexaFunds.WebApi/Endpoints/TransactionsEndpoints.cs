using System.Text.Json;
using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.QueryParameters;
using Application.DataTransfertObjects.Requests;
using Application.DataTransfertObjects.Responses;
using Application.UseCases.Transactions.Create;
using Application.UseCases.Transactions.GetById;
using Application.UseCases.Transactions.GetByQuery;
using Domain.Shared.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;


namespace HexaFunds.WebApi.Endpoints;

public static class TransactionsEndpoints
{

    public static void MapTransactionsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/transactions");

        group.MapGet("/", GetTransactionsByQuery)
            .Produces<IList<TransactionResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", GetTransactionById)
            .Produces<TransactionResponse>(StatusCodes.Status200OK)
            .WithName(nameof(GetTransactionById));

        group.MapPost("/", CreateTransaction)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<TransactionResponse>(StatusCodes.Status201Created);
    }

    // GET /api/transactions
    private static async Task<IResult> GetTransactionsByQuery(IQueryHandler<GetTransactionQuery, PagedList<TransactionResponse>> handler, [AsParameters] TransactionQueryParameters queryParameters, HttpResponse response, IOptions<JsonOptions> jsonOptions, CancellationToken cancellationToken)
    {
        var transactionsResponse = await handler.HandleAsync(new GetTransactionQuery(queryParameters), cancellationToken);

        response.Headers.Append("X-Pagination", JsonSerializer.Serialize(transactionsResponse.MetaData, jsonOptions.Value.SerializerOptions));

        return Results.Ok(transactionsResponse);
    }

    // GET /api/transactions/{id:guid}
    private static async Task<IResult> GetTransactionById(IQueryHandler<GetTransactionByIdQuery, TransactionResponse?> handler, Guid id, CancellationToken cancellationToken)
    {
        var transactionResponse = await handler.HandleAsync(new GetTransactionByIdQuery(id), cancellationToken);
        return Results.Ok(transactionResponse);
    }

    // POST /api/transactions
    private static async Task<IResult> CreateTransaction(ICommandHandler<CreateTransactionCommand, TransactionResponse> handler, TransactionRequest transactionRequest, CancellationToken cancellationToken)
    {
        var transactionResponse = await handler.HandleAsync(new CreateTransactionCommand(transactionRequest), cancellationToken);
        return Results.CreatedAtRoute(nameof(GetTransactionById), new { id = transactionResponse.Id }, transactionResponse);
    }
}