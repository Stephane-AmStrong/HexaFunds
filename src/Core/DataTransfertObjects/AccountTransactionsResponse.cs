using System.Text.Json.Serialization;

namespace DataTransfertObjects;

public record AccountTransactionsResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual CheckingAccountResponse? CheckingAccount { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual SavingsAccountResponse? SavingsAccount { get; init; }

    public virtual required IEnumerable<TransactionResponse> Transactions { get; init; }
}