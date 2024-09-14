using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataTransfertObjects;

public record TransactionRequest
{
    [Range(0, float.MaxValue, ErrorMessage = "Invalid transaction amount. Transaction amount must be greater than zero.")]
    [Required(ErrorMessage = "The AccountId is required.")]
    public required float Amount { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransactionType Type { get; init; }

    [Required(ErrorMessage = "The AccountId is required.")]
    public Guid AccountId { get; init; }

    [JsonIgnore]
    public DateTime Date { get; init; } = DateTime.UtcNow;
}
