using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.WebApi.Exceptions;
using Storage.WebApi.Models;

namespace Storage.Tests
{
    [TestClass]
    public class ContextTest
    {
        #region Private Fields

        private StorageDbContext _db = new StorageDbContext();
        private string _userId = "Test User Id";

        #endregion

        #region Tests

        [TestMethod]
        public void TestProducts()
        {
            var product_id = Create();
            Assert.IsNotNull(product_id);

            var product = Get(product_id);
            Assert.IsNotNull(product);

            var products = GetAll();
            Assert.IsNotNull(products);

            string product_old_name = product.Name;
            product.Name = string.Format("Updated Product Name on {0}", DateTime.Now.ToShortDateString());

            var updated_product = Edit(product_id, product);
            Assert.AreNotEqual(product_old_name, updated_product.Name);

            Delete(product_id);
            Assert.ThrowsException<NotFoundException>( delegate { Get(product_id); });

        }
        #endregion

        #region Private Helper Crud Operations

        private List<Product> GetAll()
        {
            var products = _db.Products.ToList();

            return products;
        }

        private Product Get(long id)
        {
            try
            {
                var product = _db.Products.Find(id);
                if (product == null)
                {
                    throw new NotFoundException(string.Format("Couldn't find product with Id '{0}'", id));
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

        private long Create()
        {
            var product = new Product()
            {
                Name = "Test Product Name",
                Description = "Test Product Description",
                Price = 20,
                Quantity = 1
            };

            _db.Products.Add(product);
            _db.SaveChanges();
            return product.Id;
        }

        private Product Edit(long Id, Product _updated_product)
        {
            var product = Get(Id);

            product = _updated_product;

            _db.Entry(product).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

            var updated_product = Get(Id);
            return updated_product;
        }

        private void Delete(long Id)
        {
            var product = Get(Id);

            _db.Products.Remove(product);
            _db.SaveChanges();
        }

        private Basket GetBasket(long Id)
        {
            try
            {
                var user_basket = _db.Baskets.Find(Id);
                if (user_basket == null)
                {
                    throw new NotFoundException(string.Format("Couldn't find basket with Id '{0}'.", Id));
                }
                return user_basket;
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
        #endregion

        #region Dispose
        ~ContextTest()
        {
            Dispose();
        }
        private void Dispose()
        {
            _db.Dispose();
        }
        #endregion
    }
}
