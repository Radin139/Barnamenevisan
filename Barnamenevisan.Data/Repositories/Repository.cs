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

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    #endregion

    #region GetById

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    #endregion

    #region Insert

    public async Task InsertAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    #endregion

    #region Update

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    #endregion

    #region Delete

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    #endregion

    #region Save

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }

    #endregion
}