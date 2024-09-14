namespace Domain.Exceptions;

public sealed class TransactionWithdrawalExceedException()
    : BadRequestException($"The requested amount exceeds the available balance.");
