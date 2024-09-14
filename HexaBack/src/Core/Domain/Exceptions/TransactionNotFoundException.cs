namespace Domain.Exceptions;

public sealed class TransactionNotFoundException(Guid id) : NotFoundException($"The transaction with the identifier {id} was not found.");
