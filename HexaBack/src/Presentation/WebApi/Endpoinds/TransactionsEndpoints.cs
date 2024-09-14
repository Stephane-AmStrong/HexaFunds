using DataTransfertObjects;

using Services.Abstractions;

namespace WebApplicationDocker.Endpoints;

public static class TransactionsEndpoints
{

    public static void MapTransactionsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/transactions");

        group.MapGet("/", GetAllTransactions)
            .Produces<IList<TransactionResponse>>()
            .Produces(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", GetTransactionById)
            .Produces<TransactionResponse>()
            .Produces(StatusCodes.Status200OK);

        group.MapGet("/statement", GetAccountStatement)
            //.WithName("statement")
            .Produces<AccountTransactionsResponse>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("account/{accountId:guid}", GetByAccountId)
            .Produces<AccountTransactionsResponse>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateTransaction)
            .Produces<TransactionResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest);
    }

    // GET /api/transactions
    private static IResult GetAllTransactions(ITransactionService transactionService)
    {
        var transactionsResponse = transactionService.GetAll();
        return Results.Ok(transactionsResponse);
    }

    // GET /api/transactions/statement
    private static async Task<IResult> GetAccountStatement(ITransactionService transactionService, [AsParameters] AccountStatementQuery accountStatementQuery, CancellationToken cancellationToken)
    {
        var transactionResponse = await transactionService.GetAccountStatementAsync(accountStatementQuery, cancellationToken);
        return Results.Ok(transactionResponse);
    }

    // GET /api/transactions/account/{accountId:guid}
    private static async Task<IResult> GetByAccountId(ITransactionService transactionService, Guid accountId, CancellationToken cancellationToken)
    {
        var transactionAccountsResponse = await transactionService.GetByAccountIdAsync(accountId, cancellationToken);
        return Results.Ok(transactionAccountsResponse);
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
