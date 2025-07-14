using Domain.Errors;

namespace Domain.Errors;

public sealed class TransactionAccountNotFoundException(Guid accountId) : BadRequestException($"The account with the identifier {accountId} was not found.");
