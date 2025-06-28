using System.Linq.Expressions;
using Barnamenevisan.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Barnamenevisan.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    #region Constructor
    
    private DbContext _db;
    private DbSet<TEntity> _dbSet;
    
    public Repository(DbContext context)
    {
        _db = context;
        _dbSet = _db.Set<TEntity>();
    }

    #endregion

    #region GetAll

    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    #endregion

    #region GetById

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<TEntity?> GetByIdAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.SingleOrDefaultAsync(predicate);
    }

    #endregion

    #region Insert

    public virtual async Task InsertAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    #endregion

    #region Update

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    #endregion

    #region Delete

    public virtual void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    #endregion

    #region Save

    public virtual async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }

    #endregion
}