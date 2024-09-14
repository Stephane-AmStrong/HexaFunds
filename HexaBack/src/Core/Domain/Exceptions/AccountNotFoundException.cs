namespace Domain.Exceptions;

public sealed class AccountNotFoundException(Guid id) : NotFoundException($"The account with the identifier {id} was not found.");
