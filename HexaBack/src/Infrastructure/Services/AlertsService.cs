#nullable enable
using Application.Abstractions.Services;
using Application.UseCases.Alerts.GetByQuery;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared.Common;
using Mapster;
using MCS.WatchTower.WebApi.DataTransferObjects.Enumerations;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;
using Microsoft.Extensions.Logging;

namespace Services;

public sealed class AlertsService(IAlertsRepository alertsRepository, ILogger<AlertsService> logger, IServersRepository serversRepository) : IAlertsService
{
    public async Task<PagedList<AlertResponse>> GetPagedListByQueryAsync(AlertQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting alerts with query parameters: {@QueryParameters}", query);
        var alerts = await alertsRepository.GetPagedListByQueryAsync(query, cancellationToken);

        var alertResponses = alerts.Adapt<List<AlertResponse>>();

        logger.LogInformation("Retrieved alerts with meta data: {@MetaData}", alerts.MetaData);
        return new PagedList<AlertResponse>(alertResponses, alerts.MetaData);
    }

    public async Task<AlertDetailedResponse?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving alert with ID: {AlertId}", id);
        Alert alert = await alertsRepository.GetByIdAsync(id, cancellationToken) ?? throw new AlertNotFoundException(id);

        Server? server = await serversRepository.GetByIdAsync(alert.ServerId, cancellationToken);

        if (server is null) logger.LogWarning("Alert {AlertId} refers to a missing server (ServerId: {ServerId})", id, alert.ServerId);

        logger.LogInformation("Alert {AlertId} retrieved.", id);

        return alert.Adapt<AlertDetailedResponse>() with
        {
            Server = server?.Adapt<ServerResponse>()
        };
    }

    public async Task<AlertResponse> CreateOrIncrementAsync(AlertCreateOrIncrementRequest alertRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing alert creation for ServerId={ServerId} and Type={AlertType}",
            alertRequest.ServerId, alertRequest.Type);

        var existingAlert = await FindExistingActiveAlert(alertRequest, cancellationToken);

        return existingAlert != null
            ? await IncrementExistingAlert(existingAlert, alertRequest, cancellationToken)
            : await CreateNewAlert(alertRequest, cancellationToken);
    }

    public async Task UpdateAsync(string id, AlertUpdateRequest alertRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating alert with ID: {AlertId}", id);
        var alert = await alertsRepository.GetByIdAsync(id, cancellationToken) ?? throw new AlertNotFoundException(id);

        alertRequest.Adapt(alert);

        logger.LogDebug("Applying updates to alert {AlertId}: Type={NewType}, Severity={NewSeverity}", id, alertRequest.Type, alertRequest.Severity);
        await alertsRepository.UpdateAsync(alert, cancellationToken);

        logger.LogInformation("Successfully updated alert with ID: {AlertId}", id);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting alert with ID: {AlertId}", id);

        var alert = await alertsRepository.GetByIdAsync(id, cancellationToken) ?? throw new AlertNotFoundException(id);

        await alertsRepository.DeleteAsync(alert, cancellationToken);

        logger.LogInformation("Successfully deleted alert with ID: {AlertId}", id);
    }

    private async Task<Alert?> FindExistingActiveAlert(AlertCreateOrIncrementRequest alertRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Checking for existing alert with ServerId={ServerId} and Type={AlertType}", alertRequest.ServerId, alertRequest.Type);

        string[] activeStatuses = [nameof(AlertStatus.Active), nameof(AlertStatus.Muted)];

        List<Alert> alerts = await alertsRepository.FindByConditionAsync(alert =>
            alert.ServerId == alertRequest.ServerId &&
            alert.Type == alertRequest.Type.ToString() &&
            activeStatuses.Contains(alert.Status),
            cancellationToken);

        return alerts.FirstOrDefault();
    }

    private async Task<AlertResponse> IncrementExistingAlert(Alert existingAlert, AlertCreateOrIncrementRequest alertRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Existing alert found with Id={AlertId}, current occurrence count: {OccurrenceCount}", existingAlert.Id, existingAlert.Occurrence);

        var updatedAlert = existingAlert with
        {
            Occurrence = existingAlert.Occurrence + 1,
            OccurredAt = DateTime.UtcNow,
            Message = alertRequest.Message ?? existingAlert.Message,
            Severity = alertRequest.Severity?.ToString() ?? existingAlert.Severity,
        };

        await alertsRepository.UpdateAsync(updatedAlert, cancellationToken);

        logger.LogInformation("Incremented occurrence count for alert {AlertId} to {NewCount}", updatedAlert.Id, updatedAlert.Occurrence);

        return updatedAlert.Adapt<AlertResponse>();
    }

    private async Task<AlertResponse> CreateNewAlert(AlertCreateOrIncrementRequest alertRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new alert of type: {AlertType} for serverId: {ServerId}", alertRequest.Type, alertRequest.ServerId);

        var alert = alertRequest.Adapt<Alert>() with
        {
            Status = nameof(AlertStatus.Active),
            Occurrence = 1,
            OccurredAt = DateTime.UtcNow,
        };

        await alertsRepository.CreateAsync(alert, cancellationToken);

        logger.LogInformation("Successfully created alert with ID: {AlertId}", alert.Id);

        return alert.Adapt<AlertResponse>();
    }
}
