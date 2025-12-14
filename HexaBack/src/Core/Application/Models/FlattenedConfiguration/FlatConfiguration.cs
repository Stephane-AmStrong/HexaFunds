using System.Text.Json.Serialization;

namespace Application.Models.FlattenedConfiguration;

public class FlatConfiguration
{
    [JsonPropertyName("Apps")]
    public List<ServerConfig> ServerConfigs { get; set; }

    [JsonPropertyName("GenerationInfo")]
    public GenerationInfo GenerationInfo { get; set; }
}
