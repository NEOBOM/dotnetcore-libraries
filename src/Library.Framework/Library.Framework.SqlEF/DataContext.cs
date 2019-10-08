using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Library.Framework.SqlEF
{
    public abstract class DataContext<TEntity>  where TEntity : class
    {
        protected readonly DbClient _db;

        public DataContext()
        {
            _db = new DbClient();
        }

        public DataContext(IConfiguration configuration)
        {
            _db = new DbClient(configuration);
        }

        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().Find(predicate);
        }

        public virtual ValueTask<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().FindAsync(predicate);
        }

        public virtual List<TEntity> FindMany(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().Where(predicate).ToList();
        }

        public virtual Task<List<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public virtual bool Insert(TEntity entity)
        {
            var result = _db.Set<TEntity>().Add(entity);
            return result.State == EntityState.Added;
        }

        public virtual async Task<bool> InsertAsync(TEntity entity)
        {
            var result = await _db.Set<TEntity>().AddAsync(entity);
            return result.State == EntityState.Added;
        }

        public virtual void InsertMany(IEnumerable<TEntity> entities)
        {
            _db.Set<TEntity>().AddRange(entities);
        }

        public virtual Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            return _db.Set<TEntity>().AddRangeAsync(entities);
        }

        public virtual bool Update(TEntity entity)
        {
            return _db.Set<TEntity>().Update(entity).State == EntityState.Modified;
        }

        public virtual void UpdateMany(IEnumerable<TEntity> entities)
        {
            _db.Set<TEntity>().UpdateRange(entities);
        }

        public virtual bool Delete(TEntity entity)
        {
            return _db.Set<TEntity>().Remove(entity).State == EntityState.Deleted;
        }

        public virtual void DeleteMany(IEnumerable<TEntity> entities)
        {
            _db.Set<TEntity>().RemoveRange(entities);
        }

        public bool Commit()
        {
            return _db.SaveChanges() > 0;
        }

        public async Task<bool> CommitAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
