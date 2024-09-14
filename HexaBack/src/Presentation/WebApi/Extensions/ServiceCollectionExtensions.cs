using System.Text.Json.Serialization;
using Domain.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Repository;
using Services;
using Services.Abstractions;
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
                    "https://localhost:4200"
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
            });

        });
    }

    public static void ConfigureBankingRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        services.AddScoped<ICheckingAccountRepository, CheckingAccountRepository>();
        services.AddScoped<ISavingsAccountRepository, SavingsAccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
    }

    public static void ConfigureBankingServices(this IServiceCollection services)
    {
        services.AddScoped<ICheckingAccountService, CheckingAccountService>();
        services.AddScoped<ISavingsAccountService, SavingsAccountService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
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

}
