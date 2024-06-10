using System;

namespace DataTransfertObjects;

public record AccountStatementResponse(string AccountType, float Balance, AccountStatementTransactionResponse[] Transactions);