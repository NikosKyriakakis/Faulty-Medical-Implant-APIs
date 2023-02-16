using System.Linq.Expressions;

namespace GenericRepository;

public interface IRepository<TEntity, in TKey>
    where TEntity : IEntity<TKey>
{
    Task<IReadOnlyCollection<TEntity>> GetAllAsync();
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter);
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter);
    Task PostAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TKey id);
}