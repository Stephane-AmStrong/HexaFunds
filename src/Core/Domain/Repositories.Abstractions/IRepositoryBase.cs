using System.Linq.Expressions;

namespace Domain.Repositories.Abstractions;

public interface IRepositoryBase<T>
{
    IQueryable<T> BaseGetAll();
    IQueryable<T> BaseFindByCondition(Expression<Func<T, bool>> expression);
    Task BaseCreateAsync(T entity, CancellationToken cancellationToken);
    void BaseUpdate(T entity);
    void BaseDelete(T entity);
}
