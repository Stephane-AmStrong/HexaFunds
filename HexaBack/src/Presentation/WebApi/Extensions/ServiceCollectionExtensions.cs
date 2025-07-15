using System.Text.Json.Serialization;
using System.Threading.Channels;
using Application.DataTransfertObjects;
using Application.Messagin.Abstractions;
using Application.Services.Abstractions;
using Application.UseCases;
using Domain.Repositories.Abstractions;
using Messaging.Channels;
using Messaging.Internals;
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
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins(
                    "http://localhost:4200",
                    "https://localhost:4200",
                    "http://localhost:5173",
                    "https://localhost:5173"
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
            });

        });
    }

    public static void ConfigureBankingRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
    }

    public static void ConfigureBankingServices(this IServiceCollection services)
    {
        services.AddScoped<IBankAccountService, BankAccountService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void ConfigureCommandProcessor(this IServiceCollection services)
    {
        services.AddSingleton(Channel.CreateUnbounded<ICommandWrapper>());
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.AddHostedService<CommandProcessor>();
    }

    public static void ConfigureHandler(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateCheckingAccountCommand, CheckingAccountResponse>, CreateCheckingAccountHandler>();
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
            using (var scope = application.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
                dbContext.Database.Migrate();
            }
        }
    }

    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<Filters.ValidationFilter<TRequest>>().ProducesValidationProblem();
    }
}
