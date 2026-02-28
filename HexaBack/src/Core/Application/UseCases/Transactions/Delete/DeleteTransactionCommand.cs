using Application.Abstractions.Handlers;

namespace Application.UseCases.Transactions.Delete;

public record DeleteTransactionCommand(Guid Id) : ICommand;
