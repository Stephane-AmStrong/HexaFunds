namespace Domain.Errors;

public sealed class HostNotFoundException(string id) : NotFoundException($"The host with the identifier '{id}' was not found.");
