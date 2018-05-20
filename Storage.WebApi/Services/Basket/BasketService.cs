using System.Collections.Generic;
using System.Threading.Tasks;
using Storage.WebApi.Exceptions;
using Storage.WebApi.Models;
using Storage.WebApi.Repository;
using Storage.WebApi.Core;
using System.Linq;
using System;

namespace Storage.WebApi.Services
{
    public class BasketService : IBasketService
    {
        #region Private Fields
        private readonly BasketRepository _repo;
        #endregion

        #region Init
        public BasketService()
        {
            _repo = new BasketRepository();
        }
        #endregion

        #region Crud Operations

        public List<Basket> GetAll()
        {
            return _repo.GetAll();
        }
        public async Task<List<Basket>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public Basket GetById(object Id)
        {
            return _repo.GetById(Id);
        }
        public async Task<Basket> GetByIdAsync(object Id)
        {
            return await _repo.GetByIdAsync(Id);
        }

        public void Delete(object Id)
        {
            var basket = GetById(Id);
            Delete(basket);
        }
        public void Delete(Basket entity)
        {
            _repo.Delete(entity);
        }

        public Basket Insert(Basket entity)
        {
            return _repo.Insert(entity);
        }

        public Basket Update(Basket entity)
        {
            _repo.Update(entity);

            return entity;
        }



        public Basket GetBasket(string userId)
        {
            var basket = _repo.Get(q => q.UserId == userId,null, "Products.Product").FirstOrDefault();
            if (basket == null)
            {
                return CreateBasket(userId);
            }
            return basket;
        }

        public Basket CreateBasket(string userId)
        {
            var basket = new Basket()
            {
                UserId = userId,
                Products = new List<BasketProduct>()
            };
            _repo.Insert(basket);
            return basket;
        }

        public Basket ClearBasket(string userId)
        {
            var basket = GetBasket(userId);
            if (basket == null)
            {
                return CreateBasket(userId);
            }
            if (basket.Products != null && basket.Products.Count > 0)
            {
                using (var basket_product_repo = new BasketProductRepository())
                {
                    foreach (var product_to_remove in basket.Products)
                    {
                        basket_product_repo.Delete(product_to_remove.Id);
                    }
                }
                basket.Products.Clear();
            }
            return basket;
        }

        public Basket AddProduct(Basket entity, long productId)
        {
            entity.Products.Add(new BasketProduct()
            {
                BasketId = entity.Id,
                ProductId = productId
            });

            Update(entity);

            return entity;
        }

        public Basket RemoveProduct(Basket entity, long productId)
        {
            try
            {
                var product_to_remove = entity.Products.Where(q => q.ProductId == productId).FirstOrDefault();
                if (product_to_remove != null)
                {
                    entity.Products.Remove(product_to_remove);
                    using (var basket_product_repo = new BasketProductRepository())
                    {
                        basket_product_repo.Delete(product_to_remove);
                    }
                    return entity;
                }
                else
                {
                    throw new NotFoundException(string.Format("Couldn't remove Product with Id '{0}' from ShoppingCart. Product not found!", productId));
                }
            }
            catch (Exception ex)
            {
                throw new NotFoundException(string.Format("Couldn't remove Product with Id '{0}' from ShoppingCart.", productId));
            }
        }

        public Basket RemoveProducts(Basket entity, long productId)
        {
            try {
                if (entity.Products == null || productId <= 0)
                {
                    return entity;
                }
                var products = entity.Products.Where(q => q.ProductId == productId).ToArray();
                if (products.Length > 0)
                {
                    using (var basket_product_repo = new BasketProductRepository())
                    {
                        foreach (var product in products)
                        {
                            basket_product_repo.Delete(product.Id);
                            entity.Products.Remove(product);
                        }
                    }
                    return GetBasket(entity.UserId);
                }
                else
                {
                    throw new NotFoundException(string.Format("Couldn't remove Products with Id '{0}' from ShoppingCart. No one Product with Id '{1}' not found!", productId, productId));
                }
            }
            catch (Exception ex)
            {
                throw new NotFoundException(string.Format("Couldn't remove Products with Id '{0}' from ShoppingCart.", productId));
            }
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
        // ~BasketService() {
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