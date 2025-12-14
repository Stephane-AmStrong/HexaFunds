using AspNetCore.Swagger.Themes;
using FluentValidation;
using Serilog;
using WebApi.Endpoints;
using WebApi.Extensions;
using WebApi.Middleware;

using WebApplicationDocker.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configures Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

builder.ConfigureDbContext();

// Add services to the container.
builder.Services.ConfigureCors(builder.Configuration);
builder.Services.ConfigureJsonOptions();
builder.Services.ConfigureValidation();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices();
builder.Services.ConfigureGlobalExceptionHandling();

builder.Services.AddHealthChecks();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.MapHealthChecks("/health");

app.ApplyMigrationsIfNotTesting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ModernStyle.Dark);
}

app.UseCors("CorsPolicy");

app.MapCheckingAccountsEndpoints();
app.MapSavingsAccountsEndpoints();
app.MapTransactionsEndpoints();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.Run();

public partial class Program { }
