using Application.Abstractions.Handlers;
using Application.UseCases.CheckingAccounts.Create;
using Application.UseCases.CheckingAccounts.Delete;
using Application.UseCases.CheckingAccounts.GetById;
using Application.UseCases.CheckingAccounts.GetByQuery;
using Application.UseCases.CheckingAccounts.Update;

namespace WebApi.Endpoints;
public static class CheckingAccountsEndpoints
{
    public static void MapCheckingAccountsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/checkingAccounts")
            .WithTags("CheckingAccounts");

        group.MapGet("/", GetByQueryParameters)
            .Produces<IList<CheckingAccountResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", GetCheckingAccountById)
            .Produces<CheckingAccountDetailedResponse>(StatusCodes.Status200OK)
            .WithName(nameof(GetCheckingAccountById));

        group.MapPost("/", CreateCheckingAccount)
            .Produces<CheckingAccountDetailedResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:guid}", DeleteCheckingAccount)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:guid}", UpdateCheckingAccount)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }

    // GET /api/checkingAccounts
    private static  async Task<IResult> GetByQueryParameters(IQueryHandler<GetCheckingAccountQuery, IList<CheckingAccountResponse>> handler, [AsParameters] CheckingAccountQuery queryParameters, HttpResponse response, CancellationToken cancellationToken)
    {
        var checkingAccountsResponse = await handler.HandleAsync(new GetCheckingAccountQuery(queryParameters), cancellationToken);

        // response.Headers.Append("X-Pagination", JsonSerializer.Serialize(checkingAccountsResponse.MetaData));

        return Results.Ok(checkingAccountsResponse);
    }

    // GET /api/checkingAccounts/{id}
    private static async Task<IResult> GetCheckingAccountById(IQueryHandler<GetCheckingAccountByIdQuery, CheckingAccountDetailedResponse?> handler, Guid id, CancellationToken cancellationToken)
    {
        var checkingAccountResponse = await handler.HandleAsync(new GetCheckingAccountByIdQuery(id), cancellationToken);
        return Results.Ok(checkingAccountResponse);
    }

    // POST /api/checkingAccounts
    private static async Task<IResult> CreateCheckingAccount(ICommandHandler<CreateCheckingAccountCommand, CheckingAccountResponse> handler, CheckingAccountCreateRequest checkingAccountRequest, CancellationToken cancellationToken)
    {
        var checkingAccountResponse = await handler.HandleAsync(new CreateCheckingAccountCommand(checkingAccountRequest), cancellationToken);
        return Results.CreatedAtRoute(nameof(GetCheckingAccountById), new { id = checkingAccountResponse.Id }, checkingAccountResponse);
    }

    // DELETE /api/checkingAccounts/{id}
    private static async Task<IResult> DeleteCheckingAccount(ICommandHandler<DeleteCheckingAccountCommand> handler, Guid id, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new DeleteCheckingAccountCommand(id), cancellationToken);
        return Results.NoContent();
    }

    // PUT /api/checkingAccounts/{id}
    private static async Task<IResult> UpdateCheckingAccount(ICommandHandler<UpdateCheckingAccountCommand> handler, Guid id, CheckingAccountUpdateRequest checkingAccountRequest, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new UpdateCheckingAccountCommand(id, checkingAccountRequest), cancellationToken);
        return Results.NoContent();
    }
}
