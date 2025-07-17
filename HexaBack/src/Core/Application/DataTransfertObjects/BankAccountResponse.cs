using Application.DataTransfertObjects;

namespace DataTransfertObjects
{
    public record BankAccountResponse : BankAccountRequest
    {
        public Guid Id { get; init; }
    }
}
