using System.Text.Json.Serialization;

namespace Application.Models.FlattenedConfiguration;

public record SpecificJob
(
    [property: JsonPropertyName("command")] string Command,
    [property: JsonPropertyName("args")] string Args,
    [property: JsonPropertyName("schedule")] string Schedule
);
