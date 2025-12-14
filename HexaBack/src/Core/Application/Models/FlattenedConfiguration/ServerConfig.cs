using System.Text.Json.Serialization;

namespace Application.Models.FlattenedConfiguration;


public record ServerConfig
(
    [property: JsonPropertyName("_id")] string Id,
    [property: JsonPropertyName("AppName")] string AppName,
    [property: JsonPropertyName("Server")] string HostName,
    [property: JsonPropertyName("Launcher")] string Launcher,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("RunMode")] string RunMode,
    [property: JsonPropertyName("startTime")] string CronStartTime,
    [property: JsonPropertyName("stopTime")] string CronStopTime,
    [property: JsonPropertyName("version")] string Version,
    [property: JsonPropertyName("base")] string Base,
    [property: JsonPropertyName("BaseChain")] string[] BaseChain,
    [property: JsonPropertyName("Port")] string Port,
    [property: JsonPropertyName("configs")] string[] Configs,
    [property: JsonPropertyName("ConfigPaths")] string[] ConfigPaths,
    [property: JsonPropertyName("Dictionary")] IReadOnlyDictionary<string, string> Dictionary,
    [property: JsonPropertyName("DictionaryLocalApp")] IReadOnlyDictionary<string, string> DictionaryLocalApp,
    [property: JsonPropertyName("dictionaries")] string[] Dictionaries,
    [property: JsonPropertyName("DictionaryPaths")] string[] DictionaryPaths,
    [property: JsonPropertyName("jobs")] JobSettings JobSettings,
    [property: JsonPropertyName("tags")] string[] Tags
);
