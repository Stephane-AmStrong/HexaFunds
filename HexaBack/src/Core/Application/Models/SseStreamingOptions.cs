#nullable enable
using System.Text.Json;

namespace Application.Models;

public record SseStreamingOptions(JsonSerializerOptions? JsonOptions);
