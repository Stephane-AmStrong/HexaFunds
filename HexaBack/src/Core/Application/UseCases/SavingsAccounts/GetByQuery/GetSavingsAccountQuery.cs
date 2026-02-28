#nullable enable
using Application.Abstractions.Handlers;
using Domain.Shared.Common;
using Application.DataTransfertObjects.QueryParameters;
using Application.DataTransfertObjects.Responses;

namespace Application.UseCases.SavingsAccounts.GetByQuery;

public record GetSavingsAccountQuery(SavingsAccountQueryParameters Parameters) : IQuery<PagedList<SavingsAccountResponse>>;
