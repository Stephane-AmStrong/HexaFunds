namespace Domain.Errors;

public sealed class ServerNotFoundException(string id) : NotFoundException($"The server with the identifier '{id}' was not found.");
