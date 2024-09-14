namespace DataTransfertObjects;

public record AccountStatementResponse(string AccountType, float Balance, TransactionResponse[] Transactions);