using Microsoft.Extensions.Logging;

namespace Application.Models;

public sealed record ServerTimeoutMonitor : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ILogger _logger;
    private bool _disposed;

    public string ServerId { get; }
    public DateTimeOffset StartTime { get; }
    public TimeSpan Timeout { get; }
    public Task TimeoutTask { get; }

    public ServerTimeoutMonitor(string serverId, TimeSpan timeout, ILogger logger)
    {
        ServerId = serverId;
        StartTime = DateTimeOffset.UtcNow;
        Timeout = timeout;
        _logger = logger;
        _cancellationTokenSource = new CancellationTokenSource();

        TimeoutTask = RunTimeoutAsync();
    }

    private async Task RunTimeoutAsync()
    {
        var expectedEndTime = StartTime.Add(Timeout);

        _logger.LogTrace("Starting {Timeout} countdown for server {ServerId} - timeout at {ExpectedEndTime}",
            Timeout, ServerId, expectedEndTime);

        try
        {
            await Task.Delay(Timeout, _cancellationTokenSource.Token);

            var actualEndTime = DateTimeOffset.UtcNow;
            _logger.LogDebug("Timeout reached for server {ServerId} at {ActualEndTime}", ServerId, actualEndTime);
        }
        catch (OperationCanceledException)
        {
            _logger.LogTrace("Timeout cancelled for server {ServerId} - heartbeat received", ServerId);
            throw;
        }
    }

    public void Cancel()
    {
        if (!_disposed && !_cancellationTokenSource.Token.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
            _logger.LogTrace("Cancelled timeout monitoring for server {ServerId}", ServerId);
        }
    }

    public void Dispose()
    {
        if (_disposed) return;

        _cancellationTokenSource?.Dispose();
        _disposed = true;

        _logger.LogTrace("Disposed timeout monitor for server {ServerId}", ServerId);
    }
}
