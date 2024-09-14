using DataTransfertObjects;

using Services.Abstractions;

namespace WebApplicationDocker.Endpoints;

public static class SavingsAccountsEndpoints
{

    public static void MapSavingsAccountsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/savingsaccounts");

        group.MapGet("", GetAllSavingsAccounts)
            .Produces<IList<SavingsAccountResponse>>()
            .Produces(StatusCodes.Status200OK);

        group.MapGet("{id:guid}", GetSavingsAccountById)
            .Produces<SavingsAccountResponse>()
            .Produces(StatusCodes.Status200OK);

        group.MapPost("", CreateSavingsAccount)
            .Produces<SavingsAccountResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("{id:guid}", DeleteSavingsAccount)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("{id:guid}", UpdateSavingsAccount)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }

    // GET /api/checkingaccounts
    private static IResult GetAllSavingsAccounts(ISavingsAccountService savingsAccountService)
    {
        var savingsAccountsResponse = savingsAccountService.GetAll();
        return Results.Ok(savingsAccountsResponse);
    }

    // GET /api/checkingaccounts/{id:guid}
    private static async Task<IResult> GetSavingsAccountById(ISavingsAccountService savingsAccountService, Guid id, CancellationToken cancellationToken)
    {
        var savingsAccountResponse = await savingsAccountService.GetByIdAsync(id, cancellationToken);
        return Results.Ok(savingsAccountResponse);
    }

    // POST /api/checkingaccounts
    private static async Task<IResult> CreateSavingsAccount(ISavingsAccountService savingsAccountService, SavingsAccountRequest savingsAccountRequest, CancellationToken cancellationToken)
    {
        var savingsAccountResponse = await savingsAccountService.CreateAsync(savingsAccountRequest, cancellationToken);
        return Results.Created(savingsAccountResponse.Id.ToString(), savingsAccountResponse);
    }

    // DELETE /api/checkingaccounts/{id:guid}
    private static async Task<IResult> DeleteSavingsAccount(ISavingsAccountService savingsAccountService, Guid id, CancellationToken cancellationToken)
    {
        await savingsAccountService.DeleteAsync(id, cancellationToken);
        return Results.NoContent();
    }

    // PUT /api/checkingaccounts/{id:guid}
    private static async Task<IResult> UpdateSavingsAccount(ISavingsAccountService savingsAccountService, Guid id, SavingsAccountRequest savingsAccountRequest, CancellationToken cancellationToken)
    {
        await savingsAccountService.UpdateAsync(id, savingsAccountRequest, cancellationToken);
        return Results.NoContent();
    }
}
