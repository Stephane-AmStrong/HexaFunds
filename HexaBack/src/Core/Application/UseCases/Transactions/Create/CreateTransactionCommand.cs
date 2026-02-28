using Application.Abstractions.Handlers;
using Application.DataTransfertObjects.Requests;
using Application.DataTransfertObjects.Responses;

namespace Application.UseCases.Transactions.Create;

public record CreateTransactionCommand(TransactionRequest Payload) : ICommand<TransactionResponse>;
