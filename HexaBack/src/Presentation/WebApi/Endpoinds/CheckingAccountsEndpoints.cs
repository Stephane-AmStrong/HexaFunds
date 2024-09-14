using DataTransfertObjects;

using Services.Abstractions;

namespace WebApplicationDocker.Endpoints;

public static class CheckingAccountsEndpoints
{

    public static void MapCheckingAccountsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/checkingaccounts");

        group.MapGet("/", GetAllCheckingAccounts)
            .Produces<IList<CheckingAccountResponse>>()
            .Produces(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", GetCheckingAccountById)
            .Produces<CheckingAccountResponse>()
            .Produces(StatusCodes.Status200OK);

        group.MapPost("/", CreateCheckingAccount)
            .Produces<CheckingAccountResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:guid}", DeleteCheckingAccount)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:guid}", UpdateCheckingAccount)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }

    // GET /api/checkingaccounts
    private static IResult GetAllCheckingAccounts(ICheckingAccountService checkingAccountService)
    {
        var checkingAccountsResponse = checkingAccountService.GetAll();
        return Results.Ok(checkingAccountsResponse);
    }

    // GET /api/checkingaccounts/{id:guid}
    private static async Task<IResult> GetCheckingAccountById(ICheckingAccountService checkingAccountService, Guid id, CancellationToken cancellationToken)
    {
        var checkingAccountResponse = await checkingAccountService.GetByIdAsync(id, cancellationToken);
        return Results.Ok(checkingAccountResponse);
    }

    // POST /api/checkingaccounts
    private static async Task<IResult> CreateCheckingAccount(ICheckingAccountService checkingAccountService, CheckingAccountRequest checkingAccountRequest, CancellationToken cancellationToken)
    {
        var checkingAccountResponse = await checkingAccountService.CreateAsync(checkingAccountRequest, cancellationToken);
        return Results.Created(checkingAccountResponse.Id.ToString(), checkingAccountResponse);
    }

    // DELETE /api/checkingaccounts/{id:guid}
    private static async Task<IResult> DeleteCheckingAccount(ICheckingAccountService checkingAccountService, Guid id, CancellationToken cancellationToken)
    {
        await checkingAccountService.DeleteAsync(id, cancellationToken);
        return Results.NoContent();
    }

    // PUT /api/checkingaccounts/{id:guid}
    private static async Task<IResult> UpdateCheckingAccount(ICheckingAccountService checkingAccountService, Guid id, CheckingAccountRequest checkingAccountRequest, CancellationToken cancellationToken)
    {
        await checkingAccountService.UpdateAsync(id, checkingAccountRequest, cancellationToken);
        return Results.NoContent();
    }
}
