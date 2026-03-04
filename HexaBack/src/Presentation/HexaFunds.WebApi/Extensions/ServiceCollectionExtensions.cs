using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Responses;
using Application.UseCases.CheckingAccounts.Create;
using Application.UseCases.CheckingAccounts.Delete;
using Application.UseCases.CheckingAccounts.GetById;
using Application.UseCases.CheckingAccounts.GetByQuery;
using Application.UseCases.CheckingAccounts.Update;
using Application.UseCases.SavingsAccounts.Create;
using Application.UseCases.SavingsAccounts.Delete;
using Application.UseCases.SavingsAccounts.GetById;
using Application.UseCases.SavingsAccounts.GetByQuery;
using Application.UseCases.SavingsAccounts.Update;
using Application.UseCases.Transactions.Create;
using Application.UseCases.Transactions.Delete;
using Application.UseCases.Transactions.GetById;
using Application.UseCases.Transactions.GetByQuery;
using Application.UseCases.Transactions.Update;
using Domain.Abstractions.Repositories;
using Domain.Shared.Common;
using FluentValidation;
using HexaFunds.WebApi.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repository;

namespace HexaFunds.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        string[] allowedOrigins = configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("X-Pagination");
            });

        });
    }
    
    public static void ConfigureBankingDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        services.AddDbContext<BankingDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(connectionString);
        });
    }

    public static void ApplyMigrationsIfNotTesting(this WebApplication application)
    {
        if (!application.Environment.IsEnvironment("Testing"))
        {
            int retries = 5;
            int retryDelay = 5; // seconds

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    using (var scope = application.Services.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
                        dbContext.Database.Migrate();
                    }
                    Console.WriteLine("Database migration successful");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database migration attempt {i + 1}/{retries} failed: {ex.Message}");
                    if (i < retries - 1)
                    {
                        Console.WriteLine($"Retrying in {retryDelay} seconds...");
                        Thread.Sleep(retryDelay * 1000);
                    }
                }
            }

            throw new Exception($"Database migration failed after {retries} attempts");
        }
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        services.AddScoped<ICheckingAccountRepository, CheckingAccountRepository>();
        services.AddScoped<ISavingsAccountRepository, SavingsAccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void ConfigureValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateCheckingAccountCommand>, CreateCheckingAccountValidator>();
        services.AddScoped<IValidator<UpdateCheckingAccountCommand>, UpdateCheckingAccountValidator>();
        services.AddScoped<IValidator<DeleteCheckingAccountCommand>, DeleteCheckingAccountValidator>();
        
        services.AddScoped<IValidator<CreateSavingsAccountCommand>, CreateSavingsAccountValidator>();
        services.AddScoped<IValidator<UpdateSavingsAccountCommand>, UpdateSavingsAccountValidator>();
        services.AddScoped<IValidator<DeleteSavingsAccountCommand>, DeleteSavingsAccountValidator>();
        
        services.AddScoped<IValidator<CreateTransactionCommand>, CreateTransactionValidator>();
        services.AddScoped<IValidator<UpdateTransactionCommand>, UpdateTransactionValidator>();
        services.AddScoped<IValidator<DeleteTransactionCommand>, DeleteTransactionValidator>();
    }

    public static void ConfigureHandlers(this IServiceCollection services)
    {
        //CheckingAccounts
        services.AddScoped<IQueryHandler<GetCheckingAccountByIdQuery, CheckingAccountResponse?>, GetCheckingAccountByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetCheckingAccountQuery, PagedList<CheckingAccountResponse>>, GetCheckingAccountQueryHandler>();

        services.AddCommandWithValidation<CreateCheckingAccountCommand, CreateCheckingAccountCommandHandler, IValidator<CreateCheckingAccountCommand>, CheckingAccountResponse>();
        services.AddCommandWithValidation<UpdateCheckingAccountCommand, UpdateCheckingAccountCommandHandler, IValidator<UpdateCheckingAccountCommand>>();
        services.AddCommandWithValidation<DeleteCheckingAccountCommand, DeleteCheckingAccountCommandHandler, IValidator<DeleteCheckingAccountCommand>>();

        //SavingsAccounts
        services.AddScoped<IQueryHandler<GetSavingsAccountByIdQuery, SavingsAccountResponse?>, GetSavingsAccountByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetSavingsAccountQuery, PagedList<SavingsAccountResponse>>, GetSavingsAccountQueryHandler>();

        services.AddCommandWithValidation<CreateSavingsAccountCommand, CreateSavingsAccountCommandHandler, IValidator<CreateSavingsAccountCommand>, SavingsAccountResponse>();
        services.AddCommandWithValidation<UpdateSavingsAccountCommand, UpdateSavingsAccountCommandHandler, IValidator<UpdateSavingsAccountCommand>>();
        services.AddCommandWithValidation<DeleteSavingsAccountCommand, DeleteSavingsAccountCommandHandler, IValidator<DeleteSavingsAccountCommand>>();

        //Transactions
        services.AddScoped<IQueryHandler<GetTransactionByIdQuery, TransactionResponse?>, GetTransactionByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetTransactionQuery, PagedList<TransactionResponse>>, GetTransactionQueryHandler>();

        services.AddCommandWithValidation<CreateTransactionCommand, CreateTransactionCommandHandler, IValidator<CreateTransactionCommand>, TransactionResponse>();
        services.AddCommandWithValidation<UpdateTransactionCommand, UpdateTransactionCommandHandler, IValidator<UpdateTransactionCommand>>();
        services.AddCommandWithValidation<DeleteTransactionCommand, DeleteTransactionCommandHandler, IValidator<DeleteTransactionCommand>>();
    }

    public static void AddOpenApiServices(this IServiceCollection services)
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
    }

    public static void UseOpenApiWithSwagger(this WebApplication app)
    {
        app.MapOpenApi("/HexaFundsWebApi/openapi/v1.json");

        // Configure OpenAPI mapping and Swagger UI
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("openapi/v1.json", "WatchTower Web API");
            options.RoutePrefix = "HexaFundsWebApi";
        });
    }

    public static void ConfigureJsonOptions(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });
    }

    public static void ConfigureGlobalExceptionHandling(this IServiceCollection services)
    {
        // services.AddScoped<EndpointLoggingMiddleware>();
        services.AddProblemDetails();
        services.AddExceptionHandler<ExceptionHandlingMiddleware>();
    }

    public static IServiceCollection AddKestrelConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KestrelServerOptions>(configuration.GetSection("Kestrel"));
        return services;
    }

    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<Filters.ValidationFilter<TRequest>>().ProducesValidationProblem();
    }
}
