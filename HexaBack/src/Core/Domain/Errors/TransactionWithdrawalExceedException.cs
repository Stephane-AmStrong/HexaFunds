using Domain.Errors;

namespace Domain.Errors;

public sealed class TransactionWithdrawalExceedException()
    : BadRequestException($"The requested amount exceeds the available balance.");
