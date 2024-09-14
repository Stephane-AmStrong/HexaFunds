namespace Domain.Exceptions;

public sealed class TransactionDepositLimitReachedException(float currentBalance, float attemptedWithdrawal)
: BadRequestException($"Transaction declined. The balance after deposit would be {currentBalance + attemptedWithdrawal}, which exceeds the deposit limit.");