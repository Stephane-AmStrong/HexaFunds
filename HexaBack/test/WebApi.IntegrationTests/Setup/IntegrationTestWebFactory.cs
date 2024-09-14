using System.Data.Common;

using DotNet.Testcontainers.Builders;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Persistence;


using Testcontainers.PostgreSql;

namespace WebApi.IntegrationTests.Setup;


[CollectionDefinition(nameof(BankingDbContext))]
public class IntegrationTestWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithDatabase("banking_db")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("pg_isready"))
        .WithCleanUp(true)
        .Build();

    public BankingDbContext Db { get; private set; } = null!;
    private DbConnection _connection = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        Db = Services.CreateScope().ServiceProvider.GetRequiredService<BankingDbContext>();
        _connection = Db.Database.GetDbConnection();
        await _connection.OpenAsync();
    }

    public new async Task DisposeAsync()
    {
        await _connection.CloseAsync();
        await _container.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Remove any existing DbContext configuration for BankingDbContext
            services.RemoveDbContext<BankingDbContext>();

            // Add the BankingDbContext with the new PostgreSQL connection string from the container
            services.AddDbContext<BankingDbContext>(options =>
            {
                options.UseNpgsql(_container.GetConnectionString());
            });

            // Ensure the database is created
            services.EnsureDbCreated<BankingDbContext>();
        });

        builder.UseEnvironment("Testing");
    }
}

public static class ServiceCollectionExtensions
{
    public static void RemoveDbContext<T>(this IServiceCollection services) where T : DbContext
    {
        var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<T>));

        if(dbContextDescriptor is not null)
        {
            services.Remove(dbContextDescriptor);
        }

        var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));

        if(dbConnectionDescriptor is not null)
        {
            services.Remove(dbConnectionDescriptor);
        }
    }

    public static void EnsureDbCreated<T>(this IServiceCollection services) where T : DbContext
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<T>();
        context.Database.EnsureCreated();
    }
}
