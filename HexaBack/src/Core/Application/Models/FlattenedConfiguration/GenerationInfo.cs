using System.Text.Json.Serialization;

namespace Application.Models.FlattenedConfiguration;

public record GenerationInfo
(
    [property: JsonPropertyName("GitSha1")] string GitSha1,
    [property: JsonPropertyName("Timestamp")] string Timestamp,
    [property: JsonPropertyName("Host")] string Host,
    [property: JsonPropertyName("CommitUrl")] string CommitUrl
);
