using Application.Abstractions.Handlers;
using Domain.Shared.Common;
using Application.DataTransfertObjects.QueryParameters;
using Application.DataTransfertObjects.Responses;

namespace Application.UseCases.CheckingAccounts.GetByQuery;

public record GetCheckingAccountQuery(CheckingAccountQueryParameters Parameters) : IQuery<PagedList<CheckingAccountResponse>>;
