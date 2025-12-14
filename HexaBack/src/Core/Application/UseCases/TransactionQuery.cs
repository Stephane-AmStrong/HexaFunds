namespace Application.UseCases;

public record class TransactionQuery(Guid? WithAccountId = null, DateTime? FromDate = null, DateTime? ToDate = null);
