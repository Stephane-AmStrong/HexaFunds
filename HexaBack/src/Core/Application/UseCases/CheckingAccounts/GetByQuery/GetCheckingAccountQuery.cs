using Application.Abstractions.Handlers;
using Domain.Shared.Common;

namespace Application.UseCases.CheckingAccounts.GetByQuery;

public record GetCheckingAccountQuery(CheckingAccountQuery Payload) : IQuery<IList<CheckingAccountResponse>>;
