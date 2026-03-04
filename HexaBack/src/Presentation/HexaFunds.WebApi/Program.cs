using FluentValidation;
using HexaFunds.WebApi.Endpoints;
using HexaFunds.WebApi.Extensions;
using HexaFunds.WebApi.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

builder.Services.AddOpenApiServices();

builder.AddCustomJsonConfigurations();

builder.Services.AddKestrelConfiguration(builder.Configuration);

// Add services to the container.
builder.Services.ConfigureCors(builder.Configuration);
builder.Services.ConfigureGlobalExceptionHandling();
builder.Services.ConfigureHandlers();
builder.Services.ConfigureJsonOptions();
builder.Services.ConfigureBankingDbContext(builder.Configuration);
builder.Services.ConfigureRepositories();
builder.Services.ConfigureValidation();

builder.Services.AddHealthChecks();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.MapHealthChecks("/health");

app.UseOpenApiWithSwagger();

app.ApplyMigrationsIfNotTesting();


app.UseCors("CorsPolicy");

app.MapCheckingAccountsEndpoints();
app.MapSavingsAccountsEndpoints();
app.MapTransactionsEndpoints();

app.UseMiddleware<EndpointLoggingMiddleware>();
// app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.Run();
