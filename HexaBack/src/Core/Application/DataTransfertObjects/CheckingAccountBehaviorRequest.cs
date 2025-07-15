using Application.DataTransfertObjects;

namespace Application.DataTransfertObjects;

public record CheckingAccountBehaviorRequest(float OverdraftLimit) : IAccountBehaviorRequest;