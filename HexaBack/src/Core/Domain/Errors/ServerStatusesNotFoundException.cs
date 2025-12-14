namespace Domain.Errors;

public sealed class ServerStatusNotFoundException(string id) : NotFoundException($"The heart beat with the identifier '{id}' was not found.");
