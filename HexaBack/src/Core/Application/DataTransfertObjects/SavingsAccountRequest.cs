namespace Application.DataTransfertObjects;

public record SavingsAccountRequest(string? AccountNumber, IAccountBehaviorRequest AccountBehavior, float BalanceCeiling);
