using System.Linq.Expressions;

namespace Barnamenevisan.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity?> GetByIdAsync(Expression<Func<TEntity, bool>> predicate);
    Task InsertAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task SaveAsync();
}