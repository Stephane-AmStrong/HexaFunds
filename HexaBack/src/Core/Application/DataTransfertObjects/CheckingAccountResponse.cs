
namespace Application.DataTransfertObjects;

public record CheckingAccountResponse(Guid Id, string AccountNumber, IAccountBehaviorRequest AccountBehavior) : BankAccountResponse(Id, AccountNumber, AccountBehavior);
