using Application.DataTransfertObjects;
using Application.Messagin.Abstractions;

namespace Application.UseCases;

public record CreateCheckingAccountCommand : BankAccountRequest, ICommand<CheckingAccountResponse>
{
    public float OverdraftLimit { get; set; }

    public CreateCheckingAccountCommand(string? accountNumber, float overdraftLimit) : base(accountNumber)
    {
        AccountNumber = accountNumber;
        OverdraftLimit = overdraftLimit;
    }
}
