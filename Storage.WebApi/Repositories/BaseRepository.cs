using Storage.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Storage.WebApi.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Private Fields
        private readonly StorageDbContext context;
        internal DbSet<TEntity> dbSet;
        #endregion

        #region Init
        public BaseRepository(StorageDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        #endregion

        #region IQueryable 
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        #endregion

        #region Crud Operations

        public virtual List<TEntity> GetAll()
        {
            return Get().ToList();
        }
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual TEntity GetById(object Id)
        {
            return dbSet.Find(Id);
        }
        public virtual async Task<TEntity> GetByIdAsync(object Id)
        {
            return await dbSet.FindAsync(Id);
        }

        public virtual TEntity Insert(TEntity entity)
        {
            var created_entity = dbSet.Add(entity);
            SaveChanges();
            return created_entity;
        }

        public virtual void Delete(object Id)
        {
            TEntity entityToDelete = dbSet.Find(Id);
            Delete(entityToDelete);
        }
        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State != EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            if (context.Entry(entityToDelete).State != EntityState.Deleted)
            {
                context.Entry(entityToDelete).State = EntityState.Deleted;
            }
            dbSet.Remove(entityToDelete);
            SaveChanges();
        }

        public virtual TEntity Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            if (context.Entry(entityToUpdate).State != EntityState.Modified)
            {
                context.Entry(entityToUpdate).State = EntityState.Modified;
            }
            SaveChanges();

            return entityToUpdate;
        }

        public virtual void SaveChanges()
        {
            context.SaveChanges();
        }
        public virtual async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                    dbSet = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BaseRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}