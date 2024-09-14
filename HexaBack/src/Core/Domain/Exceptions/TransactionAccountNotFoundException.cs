namespace Domain.Exceptions;

public sealed class TransactionAccountNotFoundException(Guid accountId) : BadRequestException($"The account with the identifier {accountId} was not found.");
