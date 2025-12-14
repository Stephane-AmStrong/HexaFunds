using System.Text.Json.Serialization;

namespace Application.Models.FlattenedConfiguration;

public record JobSettings
(
    [property: JsonPropertyName("common")] string[] Common,
    [property: JsonPropertyName("specific"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    Dictionary<string, SpecificJob> Specific
);
