namespace Domain.Errors;

public class BadRequestException(string message) : Exception(message);
