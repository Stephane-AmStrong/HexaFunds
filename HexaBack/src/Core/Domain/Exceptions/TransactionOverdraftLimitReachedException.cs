namespace Domain.Exceptions;

public sealed class TransactionOverdraftLimitReachedException(float currentBalance, float attemptedWithdrawal) 
    : BadRequestException($"Transaction declined. The balance after withdrawal would be {currentBalance - attemptedWithdrawal:C}, which exceeds the overdraft limit.");
