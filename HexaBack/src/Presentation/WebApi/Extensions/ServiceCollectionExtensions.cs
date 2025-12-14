using System.Text.Json.Serialization;
using Application.Abstractions.Services;
using Application.UseCases.CheckingAccounts.Create;
using Application.UseCases.CheckingAccounts.Delete;
using Application.UseCases.CheckingAccounts.Update;
using Domain.Abstractions.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Repository;
using Services;
using WebApi.Middleware;

namespace WebApi.Extensions;

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
                .AllowAnyMethod();
            });

        });
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBankAccountsRepository, BankAccountsRepository>();
        services.AddScoped<ICheckingAccountsRepository, CheckingAccountsRepository>();
        services.AddScoped<ISavingsAccountsRepository, SavingsAccountsRepository>();
        services.AddScoped<ITransactionsRepository, TransactionsRepository>();
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<ICheckingAccountsService, CheckingAccountsService>();
        services.AddScoped<ISavingsAccountsService, SavingsAccountsService>();
        // services.AddScoped<ITransactionsService, TransactionsService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void ConfigureValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateCheckingAccountCommand>, CreateCheckingAccountValidator>();
        services.AddScoped<IValidator<UpdateCheckingAccountCommand>, UpdateCheckingAccountValidator>();
        services.AddScoped<IValidator<DeleteCheckingAccountCommand>, DeleteCheckingAccountValidator>();
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
                        c.SwaggerDoc("v1", new OpenApiInfo { Title = "HexaFunds Web API", Version = "v1" }));
    }

    public static void ConfigureJsonOptions(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
    }

    public static void ConfigureGlobalExceptionHandling(this IServiceCollection services)
    {
        services.AddTransient<ExceptionHandlingMiddleware>();
    }

    public static void ConfigureDbContext(this WebApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Services.AddDbContext<BankingDbContext>(optionsBuilder =>
        {
            var connectionString = applicationBuilder.Configuration.GetConnectionString("Database");
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

    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<Filters.ValidationFilter<TRequest>>().ProducesValidationProblem();
    }
}
