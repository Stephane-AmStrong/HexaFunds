#nullable enable
using Application.Abstractions.Services;
using Application.Models.FlattenedConfiguration;
using Microsoft.Extensions.Logging;

namespace Services;

public class FlatConfigurationService(ILogger<FlatConfigurationService> logger, IJsonFileReader jsonFileReader, string filePath) : IFlatConfigurationService
{
    private const int ParallelServerThreshold = 50;

    public async Task<FlatConfiguration?> LoadEnvironmentAsync()
    {
        try
        {
            FlatConfiguration? flatConfiguration = await jsonFileReader.DeserializeFileAsync<FlatConfiguration>(filePath);
            return flatConfiguration;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get FlatConfiguration from {FilePath}", filePath);
            return null;
        }
    }

    public async Task<ServerConfig?> FindServerConfigByIdentifierAsync(string hostName, string alias)
    {
        IList<ServerConfig> serversConfig = await FindServerConfigsByIdentifiersAsync(new List<(string hostName, string alias)> { (hostName, alias) });
        return serversConfig.FirstOrDefault();
    }

    public async Task<IList<ServerConfig>> FindServerConfigsByIdentifiersAsync(IList<(string hostName, string alias)> serverIdentifiers = default)
    {
        FlatConfiguration? flatConfiguration = await LoadEnvironmentAsync();
        if (flatConfiguration?.ServerConfigs == null)
        {
            return Array.Empty<ServerConfig>();
        }

        if (serverIdentifiers is null || !serverIdentifiers.Any())
        {
            return flatConfiguration.ServerConfigs.ToList();
        }

        var idSet = serverIdentifiers.Select(serverIdentifier => $"{serverIdentifier.hostName}-{serverIdentifier.alias}")
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        bool useParallel = flatConfiguration.ServerConfigs.Count >= ParallelServerThreshold;

        IEnumerable<ServerConfig> query = flatConfiguration.ServerConfigs.AsEnumerable();

        if (useParallel)
        {
            query = query.AsParallel();
        }

        return query
            .Where(server => server.Id is not null && idSet.Contains(server.Id))
            .ToList();
    }
}
