using DataTransfertObjects;

using Services.Abstractions;

namespace WebApplicationDocker.Endpoints;

public static class TransactionsEndpoints
{

    public static void MapTransactionsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/transactions");

        group.MapGet("/", GetAllTransactions)
            .Produces<IList<TransactionResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", GetTransactionById)
            .Produces<TransactionResponse>(StatusCodes.Status200OK);

        group.MapPost("/", CreateTransaction)
            .Produces<TransactionResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);
    }

    // GET /api/transactions
    private static IResult GetAllTransactions(ITransactionService transactionService, [AsParameters] TransactionQuery transactionQuery)
    {
        var transactionsResponse = transactionService.Get(transactionQuery);
        return Results.Ok(transactionsResponse);
    }

    // GET /api/transactions/{id:guid}
    private static async Task<IResult> GetTransactionById(ITransactionService transactionService, Guid id, CancellationToken cancellationToken)
    {
        var transactionResponse = await transactionService.GetByIdAsync(id, cancellationToken);
        return Results.Ok(transactionResponse);
    }

    // POST /api/transactions
    private static async Task<IResult> CreateTransaction(ITransactionService transactionService, TransactionRequest transactionRequest, CancellationToken cancellationToken)
    {
        var transactionResponse = await transactionService.CreateAsync(transactionRequest, cancellationToken);
        return Results.Created(transactionResponse.Id.ToString(), transactionResponse);
    }
}
