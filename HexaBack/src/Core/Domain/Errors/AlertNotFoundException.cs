namespace Domain.Errors;

public sealed class AlertNotFoundException(string id) : NotFoundException($"The alert with the identifier '{id}' was not found.");
