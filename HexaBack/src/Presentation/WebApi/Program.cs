using AspNetCore.Swagger.Themes;

using WebApi.Extensions;
using WebApi.Middleware;

using WebApplicationDocker.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureDbContext();

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureJsonOptions();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureBankingRepositories();
builder.Services.ConfigureBankingServices();
builder.Services.ConfigureGlobalExceptionHandling();

var app = builder.Build();

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
