#nullable enable
using Application.Models.FlattenedConfiguration;

namespace Application.Abstractions.Services;

public interface IFlatConfigurationService
{
    Task<FlatConfiguration?> LoadEnvironmentAsync();
    Task<ServerConfig?> FindServerConfigByIdentifierAsync(string hostName, string alias);
    Task<IList<ServerConfig>> FindServerConfigsByIdentifiersAsync(IList<(string hostName, string alias)> serverIdentifiers = default!);
}
