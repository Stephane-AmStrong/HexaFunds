
using Application.DataTransfertObjects;
using Application.Messagin.Abstractions;
using Application.Services.Abstractions;
using Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Endpoinds;

public static class BankAccountsEndpoints
{

    public static void MapBankAccountsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/bank-accounts");

        group.MapGet("/", GetAllBankAccounts)
            .Produces<IList<BankAccountResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", GetBankAccountById)
            .Produces<BankAccountResponse>(StatusCodes.Status200OK);

        // group.MapPost("/", CreateBankAccount)
        //     .WithRequestValidation<BankAccountRequest>()
        //     .Produces<BankAccountResponse>(StatusCodes.Status201Created)
        //     .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        app.MapPost("/api/checking-accounts", CreateCheckingAccount)
            .WithRequestValidation<CheckingAccountBehaviorRequest>()
            .Produces<CheckingAccountResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        // app.MapPost("/api/savings-accounts", CreateSavingsAccount)
        //     .WithRequestValidation<SavingsAccountRequest>()
        //     .Produces<SavingsAccountResponse>(StatusCodes.Status201Created)
        //     .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:guid}", DeleteBankAccount)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:guid}", UpdateBankAccount)
            .WithRequestValidation<BankAccountRequest>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }

    // GET /api/bankaccounts
    private static IResult GetAllBankAccounts(IBankAccountService bankAccountService)
    {
        var bankAccountsResponse = bankAccountService.GetAll();
        return Results.Ok(bankAccountsResponse);
    }

    // GET /api/bankaccounts/{id:guid}
    private static async Task<IResult> GetBankAccountById(IBankAccountService bankAccountService, Guid id, CancellationToken cancellationToken)
    {
        var bankAccountResponse = await bankAccountService.GetByIdAsync(id, cancellationToken);
        return Results.Ok(bankAccountResponse);
    }

    // POST /api/bankaccounts
    private static async Task<IResult> CreateBankAccount(IBankAccountService bankAccountService, BankAccountRequest bankAccountRequest, CancellationToken cancellationToken)
    {
        var bankAccountResponse = await bankAccountService.CreateAsync(bankAccountRequest, cancellationToken);
        return Results.Created(bankAccountResponse.Id.ToString(), bankAccountResponse);
    }

    // POST /api/bankaccounts
    private static async Task<IResult> CreateCheckingAccount(ICommandDispatcher dispatcher, [FromBody] CreateCheckingAccountCommand command, CancellationToken cancellationToken)
    {
        // var bankAccountRequest = new(checkingAccountRequest.AccountNumber, )
        var bankAccountResponse = await dispatcher.DispatchAsync<CreateCheckingAccountCommand, CheckingAccountResponse>(command);
        return Results.Created(bankAccountResponse.Id.ToString(), bankAccountResponse);
    }

    // POST /api/bankaccounts
    // private static async Task<IResult> CreateSavingsAccount(IBankAccountService bankAccountService, SavingsAccountRequest savingsAccountRequest, CancellationToken cancellationToken)
    // {
    //     var bankAccountResponse = await bankAccountService.CreateAsync(savingsAccountRequest, cancellationToken);
    //     return Results.Created(bankAccountResponse.Id.ToString(), bankAccountResponse);
    // }

    // DELETE /api/bankaccounts/{id:guid}
    private static async Task<IResult> DeleteBankAccount(IBankAccountService bankAccountService, Guid id, CancellationToken cancellationToken)
    {
        await bankAccountService.DeleteAsync(id, cancellationToken);
        return Results.NoContent();
    }

    // PUT /api/bankaccounts/{id:guid}
    private static async Task<IResult> UpdateBankAccount(IBankAccountService bankAccountService, Guid id, BankAccountRequest bankAccountRequest, CancellationToken cancellationToken)
    {
        await bankAccountService.UpdateAsync(id, bankAccountRequest, cancellationToken);
        return Results.NoContent();
    }
}
