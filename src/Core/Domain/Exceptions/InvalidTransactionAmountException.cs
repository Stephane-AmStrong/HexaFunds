namespace Domain.Exceptions;

public sealed class InvalidTransactionAmountException() 
    : BadRequestException($"Invalid transaction amount. Transaction amount must greater than zero.");
