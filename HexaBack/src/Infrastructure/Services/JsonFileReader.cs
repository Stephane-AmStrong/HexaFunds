#nullable enable
using System.Text.Json;
using Application.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace Services;

public class JsonFileReader(ILogger<JsonFileReader> logger) : IJsonFileReader
{
    private static readonly JsonSerializerOptions s_options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<T?> DeserializeFileAsync<T>(string filePath)
    {
        try
        {
            logger.LogInformation("Reading file from {FilePath}", filePath);

            await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return await JsonSerializer.DeserializeAsync<T>(stream, s_options);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error reading file: {FilePath}", filePath);
            return default;
        }
    }
}
