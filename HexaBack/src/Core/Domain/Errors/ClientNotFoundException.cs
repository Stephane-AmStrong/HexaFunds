namespace Domain.Errors;

public sealed class ClientNotFoundException(string id) : NotFoundException($"The client with the identifier '{id}' was not found.");
