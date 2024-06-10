namespace Domain.Exceptions;

public sealed class TransactionNotFoundException(Guid id) : BadRequestException($"The transaction with the identifier {id} was not found.");
