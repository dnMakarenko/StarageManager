using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Storage.WebApi.Services
{
    public interface IService<TEntity> : IDisposable where TEntity : class
    {
        List<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync();
        TEntity GetById(object Id);
        Task<TEntity> GetByIdAsync(object Id);

        TEntity Insert(TEntity entity);
        void Delete(object Id);
        void Delete(TEntity entity);
        TEntity Update(TEntity entity);

        //void SaveChanges();
        //Task SaveChangesAsync();
    }
}