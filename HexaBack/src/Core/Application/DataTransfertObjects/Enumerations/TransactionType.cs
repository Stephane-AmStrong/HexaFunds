using System.Text.Json.Serialization;

namespace Application.DataTransfertObjects.Enumerations;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionType
{
    Credit,
    Debit,
}
