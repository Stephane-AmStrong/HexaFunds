using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Abstractions.Handlers;
using Application.Abstractions.Services;
using Application.Models;
using Application.UseCases.Alerts.CreateOrIncrement;
using Application.UseCases.Alerts.Delete;
using Application.UseCases.Alerts.GetById;
using Application.UseCases.Alerts.GetByQuery;
using Application.UseCases.Alerts.Update;
using Application.UseCases.Clients.Create;
using Application.UseCases.Clients.Delete;
using Application.UseCases.Clients.GetById;
using Application.UseCases.Clients.GetByQuery;
using Application.UseCases.Clients.Update;
using Application.UseCases.Connections.Establish;
using Application.UseCases.Connections.GetById;
using Application.UseCases.Connections.GetByQuery;
using Application.UseCases.Connections.Terminate;
using Application.UseCases.Hosts.Create;
using Application.UseCases.Hosts.Delete;
using Application.UseCases.Hosts.GetById;
using Application.UseCases.Hosts.GetByName;
using Application.UseCases.Hosts.GetByQuery;
using Application.UseCases.Hosts.GetServerOfHost;
using Application.UseCases.Hosts.GetWithServersByQuery;
using Application.UseCases.Hosts.Update;
using Application.UseCases.Servers.Create;
using Application.UseCases.Servers.Delete;
using Application.UseCases.Servers.GetById;
using Application.UseCases.Servers.GetByQuery;
using Application.UseCases.Servers.Update;
using Application.UseCases.ServerStatuses.Create;
using Application.UseCases.ServerStatuses.GetById;
using Application.UseCases.ServerStatuses.GetByQuery;
using Domain.Abstractions.Events;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Shared.Common;
using FluentValidation;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using MongoDB.Driver;
using Persistence.Events;
using Persistence.Repository;
using Services;
using Watchtower.WebApi.Middleware;
using Watchtower.WebApi.Utilities;
using Host = Domain.Entities.Host;

namespace Watchtower.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        string[] allowedOrigins = configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                //builder.WithOrigins(allowedOrigins)
                builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });

        });
    }

    public static void ConfigureMongoDB(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDB");
        var databaseName = configuration.GetConnectionString("DatabaseName");

        services.AddSingleton<IMongoClient>(serviceProvider =>
        {
            return new MongoClient(connectionString);
        });

        services.AddScoped(serviceProvider =>
        {
            var client = serviceProvider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(databaseName);
        });
    }

    public static void ConfigureFlatConfigurationSync(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFlatConfigurationService>(serviceProvider =>
        {
            string mcsConfigFlatPath = configuration["SharedFilePaths:McsConfigFlat"]!;
            var logger = serviceProvider.GetRequiredService<ILogger<FlatConfigurationService>>();
            var jsonReader = serviceProvider.GetRequiredService<IJsonFileReader>();

            return new FlatConfigurationService(logger, jsonReader, mcsConfigFlatPath);
        });
    }

    public static void ConfigureValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateOrIncrementAlertCommand>, CreateOrIncrementAlertValidator>();
        services.AddScoped<IValidator<UpdateAlertCommand>, UpdateAlertValidator>();
        services.AddScoped<IValidator<DeleteAlertCommand>, DeleteAlertValidator>();

        services.AddScoped<IValidator<CreateClientCommand>, CreateClientValidator>();
        services.AddScoped<IValidator<UpdateClientCommand>, UpdateClientValidator>();
        services.AddScoped<IValidator<DeleteClientCommand>, DeleteClientValidator>();

        services.AddScoped<IValidator<CreateHostCommand>, CreateHostValidator>();
        services.AddScoped<IValidator<UpdateHostCommand>, UpdateHostValidator>();
        services.AddScoped<IValidator<DeleteHostCommand>, DeleteHostValidator>();

        services.AddScoped<IValidator<EstablishConnectionCommand>, EstablishConnectionValidator>();
        services.AddScoped<IValidator<TerminateConnectionCommand>, TerminateConnectionValidator>();

        services.AddScoped<IValidator<CreateServerCommand>, CreateServerValidator>();
        services.AddScoped<IValidator<UpdateServerCommand>, UpdateServerValidator>();
        services.AddScoped<IValidator<DeleteServerCommand>, DeleteServerValidator>();

        services.AddScoped<IValidator<CreateServerStatusCommand>, CreateServerStatusValidator>();
    }

    public static void ConfigureHandlers(this IServiceCollection services)
    {
        //Alerts
        services.AddScoped<IQueryHandler<GetAlertByIdQuery, AlertDetailedResponse>, GetAlertByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetAlertQuery, PagedList<AlertResponse>>, GetAlertQueryHandler>();

        services.AddCommandWithValidation<CreateOrIncrementAlertCommand, CreateOrIncrementAlertCommandHandler, IValidator<CreateOrIncrementAlertCommand>, AlertResponse>();
        services.AddCommandWithValidation<UpdateAlertCommand, UpdateAlertCommandHandler, IValidator<UpdateAlertCommand>>();
        services.AddCommandWithValidation<DeleteAlertCommand, DeleteAlertCommandHandler, IValidator<DeleteAlertCommand>>();

        //Clients
        services.AddScoped<IQueryHandler<GetClientByIdQuery, ClientDetailedResponse>, GetClientByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetClientQuery, PagedList<ClientResponse>>, GetClientQueryHandler>();

        services.AddCommandWithValidation<CreateClientCommand, CreateClientCommandHandler, IValidator<CreateClientCommand>, ClientResponse>();
        services.AddCommandWithValidation<UpdateClientCommand, UpdateClientCommandHandler, IValidator<UpdateClientCommand>>();
        services.AddCommandWithValidation<DeleteClientCommand, DeleteClientCommandHandler, IValidator<DeleteClientCommand>>();

        //Connections
        services.AddScoped<IQueryHandler<GetConnectionByIdQuery, ConnectionDetailedResponse>, GetConnectionByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetConnectionQuery, PagedList<ConnectionResponse>>, GetConnectionQueryHandler>();

        services.AddCommandWithValidation<EstablishConnectionCommand, EstablishConnectionCommandHandler, IValidator<EstablishConnectionCommand>, ConnectionResponse>();
        services.AddCommandWithValidation<TerminateConnectionCommand, TerminateConnectionCommandHandler, IValidator<TerminateConnectionCommand>>();

        //Hosts
        services.AddScoped<IQueryHandler<GetHostByIdQuery, HostDetailedResponse>, GetHostByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetHostByNameQuery, HostDetailedResponse>, GetHostByNameQueryHandler>();
        services.AddScoped<IQueryHandler<GetServerOfHostQuery, ServerDetailedResponse>, GetServerOfHostQueryHandler>();
        services.AddScoped<IQueryHandler<GetHostQuery, PagedList<HostResponse>>, GetHostQueryHandler>();
        services.AddScoped<IQueryHandler<GetHostWithServersQuery, PagedList<HostDetailedResponse>>, GetHostWithServersQueryHandler>();

        services.AddCommandWithValidation<CreateHostCommand, CreateHostCommandHandler, IValidator<CreateHostCommand>, HostResponse>();
        services.AddCommandWithValidation<UpdateHostCommand, UpdateHostCommandHandler, IValidator<UpdateHostCommand>>();
        services.AddCommandWithValidation<DeleteHostCommand, DeleteHostCommandHandler, IValidator<DeleteHostCommand>>();

        //ServerStatuses
        services.AddScoped<IQueryHandler<GetServerStatusByIdQuery, ServerStatusDetailedResponse>, GetServerStatusByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetServerStatusQuery, PagedList<ServerStatusResponse>>, GetServerStatusQueryHandler>();

        services.AddCommandWithValidation<CreateServerStatusCommand, CreateServerStatusCommandHandler, IValidator<CreateServerStatusCommand>, ServerStatusResponse>();

        //Servers
        services.AddScoped<IQueryHandler<GetServerByIdQuery, ServerDetailedResponse>, GetServerByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetServerQuery, PagedList<ServerResponse>>, GetServerQueryHandler>();

        services.AddCommandWithValidation<CreateServerCommand, CreateServerCommandHandler, IValidator<CreateServerCommand>, ServerResponse>();
        services.AddCommandWithValidation<UpdateServerCommand, UpdateServerCommandHandler, IValidator<UpdateServerCommand>>();
        services.AddCommandWithValidation<DeleteServerCommand, DeleteServerCommandHandler, IValidator<DeleteServerCommand>>();
    }

    public static void ConfigureDomainEventHandlers(this IServiceCollection services)
    {
        services.AddScoped<IEventHandler<CreatedEvent<Alert>>, AlertCreatedEventHandler>();
        services.AddScoped<IEventHandler<UpdatedEvent<Alert>>, AlertUpdatedEventHandler>();
        services.AddScoped<IEventHandler<DeletedEvent<Alert>>, AlertDeletedEventHandler>();

        services.AddScoped<IEventHandler<CreatedEvent<Client>>, ClientCreatedEventHandler>();
        services.AddScoped<IEventHandler<UpdatedEvent<Client>>, ClientUpdatedEventHandler>();
        services.AddScoped<IEventHandler<DeletedEvent<Client>>, ClientDeletedEventHandler>();

        services.AddScoped<IEventHandler<CreatedEvent<Host>>, HostCreatedEventHandler>();
        services.AddScoped<IEventHandler<UpdatedEvent<Host>>, HostUpdatedEventHandler>();
        services.AddScoped<IEventHandler<DeletedEvent<Host>>, HostDeletedEventHandler>();

        services.AddScoped<IEventHandler<CreatedEvent<Connection>>, ConnectionEstablishedEventHandler>();
        services.AddScoped<IEventHandler<DeletedEvent<Connection>>, ConnectionTerminatedEventHandler>();

        services.AddScoped<IEventHandler<CreatedEvent<Server>>, ServerCreatedEventHandler>();
        services.AddScoped<IEventHandler<UpdatedEvent<Server>>, ServerUpdatedEventHandler>();
        services.AddScoped<IEventHandler<DeletedEvent<Server>>, ServerDeletedEventHandler>();

        services.AddScoped<IEventHandler<CreatedEvent<ServerStatus>>, ServerStatusCreatedEventHandler>();
    }

    public static IServiceCollection AddEventStreaming(this IServiceCollection services)
    {
        services.AddSingleton<IEventStreamingService<AlertResponse>, EventStreamingService<AlertResponse>>();
        services.AddSingleton<IEventStreamingService<ClientResponse>, EventStreamingService<ClientResponse>>();
        services.AddSingleton<IEventStreamingService<ConnectionResponse>, EventStreamingService<ConnectionResponse>>();
        services.AddSingleton<IEventStreamingService<HostResponse>, EventStreamingService<HostResponse>>();
        services.AddSingleton<IEventStreamingService<ServerStatusResponse>, EventStreamingService<ServerStatusResponse>>();
        services.AddSingleton<IEventStreamingService<ServerResponse>, EventStreamingService<ServerResponse>>();

        services.AddScoped<SseStreamer>(provider => new SseStreamer(new SseStreamingOptions
        (
            JsonOptions: new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            }
        ), provider.GetRequiredService<ILogger<SseStreamer>>()));

        return services;
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAlertsRepository, AlertsRepository>();
        services.AddScoped<IClientsRepository, ClientsRepository>();
        services.AddScoped<IConnectionsRepository, ConnectionsRepository>();
        services.AddScoped<IHostsRepository, HostsRepository>();
        services.AddScoped<IServerStatusesRepository, ServerStatusesRepository>();
        services.AddScoped<IServersRepository, ServersRepository>();
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<HeartbeatListenerService>();
        services.AddSingleton<IHeartbeatNotificationService>(provider => provider.GetRequiredService<HeartbeatListenerService>());
        services.AddHostedService<HeartbeatListenerService>(provider => provider.GetRequiredService<HeartbeatListenerService>());

        services.AddScoped<IAlertsService, AlertsService>();
        services.AddScoped<IClientsService, ClientsService>();
        services.AddScoped<IConnectionsService, ConnectionsService>();
        services.AddScoped<IEventsDispatcher, EventsDispatcher>();
        services.AddScoped<IHostsService, HostsService>();
        services.AddScoped<IJsonFileReader, JsonFileReader>();
        services.AddScoped<IHostConfigurationSyncService, HostConfigurationSyncService>();
        services.AddScoped<IServerConfigurationSyncService, ServerConfigurationSyncService>();
        services.AddScoped<IServersService, ServersService>();
        services.AddScoped<IServerStatusesService, ServerStatusesService>();

        //HostedService
        services.AddHostedService<WatchtowerBootstrapService>();
    }

    public static void AddOpenApiServices(this IServiceCollection services)
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
    }

    public static void UseOpenApiWithSwagger(this WebApplication app)
    {
        app.MapOpenApi("/WatchtowerWebApi/openapi/v1.json");

        // Configure OpenAPI mapping and Swagger UI
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("openapi/v1.json", "WatchTower Web API");
            options.RoutePrefix = "WatchtowerWebApi";
        });
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
        services.AddScoped<EndpointLoggingMiddleware>();
        services.AddScoped<ExceptionHandlingMiddleware>();
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
