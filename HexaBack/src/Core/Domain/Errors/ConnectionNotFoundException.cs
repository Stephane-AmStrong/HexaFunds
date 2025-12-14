namespace Domain.Errors;

public sealed class ConnectionNotFoundException(string id) : NotFoundException($"The connection with the identifier '{id}' was not found.");
