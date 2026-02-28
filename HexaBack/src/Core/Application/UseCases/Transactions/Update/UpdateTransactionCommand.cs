using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Requests;

namespace Application.UseCases.Transactions.Update;

public record UpdateTransactionCommand(Guid Id, TransactionRequest Payload) : ICommand;
