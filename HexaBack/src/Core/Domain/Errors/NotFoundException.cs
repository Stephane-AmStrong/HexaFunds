namespace Domain.Errors;

public abstract class NotFoundException(string message) : Exception(message);
