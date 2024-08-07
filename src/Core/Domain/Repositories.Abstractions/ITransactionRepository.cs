﻿using System.Linq.Expressions;

using Domain.Entities;

namespace Domain.Repositories.Abstractions;

public interface ITransactionRepository
{
    IEnumerable<Transaction> GetAll();
    IQueryable<Transaction> GetByCondition(Expression<Func<Transaction, bool>> expression);
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(Transaction transaction, CancellationToken cancellationToken);
}
