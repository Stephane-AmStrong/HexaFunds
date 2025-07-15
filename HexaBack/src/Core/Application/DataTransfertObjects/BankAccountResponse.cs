using Application.DataTransfertObjects;

namespace Application.DataTransfertObjects;

public record BankAccountResponse(Guid Id, string AccountNumber, IAccountBehaviorRequest AccountBehavior);