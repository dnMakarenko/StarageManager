using System.Collections.Generic;
using System.Threading.Tasks;
using Storage.WebApi.Core;
using Storage.WebApi.Models;
using Storage.WebApi.Exceptions;
using System.Linq;
using System.Data.Entity;
using System;
using Storage.WebApi.Repository;

namespace Storage.WebApi.Services
{
    public class ProductService : IProductService
    {
        #region Private Fields
        private readonly ProductRepository _repo;
        #endregion

        #region Init
        public ProductService()
        {
            _repo = new ProductRepository();
        }
        #endregion

        #region Crud Operations
        public List<Product> GetAll()
        {
            return _repo.GetAll();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public Product GetById(object Id)
        {
            try
            {
                var product = _repo.GetById(Id);
                if (product == null)
                {
                    throw new NotFoundException();
                }
                return product;
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Product> GetByIdAsync(object Id)
        {
            return await _repo.GetByIdAsync(Id);
        }

        public Product Insert(Product entity)
        {
            return _repo.Insert(entity);
        }

        public void Delete(object Id)
        {
            var product = GetById(Id);
            Delete(product);
        }

        public void Delete(Product entity)
        {
            _repo.Delete(entity);
        }

        public Product Update(Product entity)
        {
            _repo.Update(entity);

            return entity;
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
                    _repo.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ProductService() {
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