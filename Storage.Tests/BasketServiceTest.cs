using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Storage.WebApi.Services;
using Storage.WebApi.Models;
using Storage.WebApi.Core;
using Storage.WebApi.Exceptions;

namespace Storage.Tests
{
    [TestClass]
    public class BasketServiceTest
    {
        #region Private Fields
        private static readonly string _userId = "Test User Id";
        private readonly IBasketService _basket_service = new BasketService();
        private readonly IProductService _product_service = new ProductService();
        #endregion

        #region Tests

        [TestMethod]
        public void TestBasketService()
        {
            var products_indexes = CreateProducts();
            Assert.AreNotEqual(0, products_indexes.FirstOrDefault());

            var basket = _basket_service.Insert(TestBasket);
            Assert.AreNotEqual(0, basket.Id);

            foreach (var prod_id in products_indexes)
            {
                basket.Products.Add(new BasketProduct()
                {
                    BasketId = basket.Id,
                    ProductId = prod_id,
                });
            }

            _basket_service.Update(basket);
            var updated_basket = _basket_service.GetById(basket.Id);
            Assert.AreNotEqual(0, updated_basket.Products.Count);

            _basket_service.Delete(updated_basket.Id);
            Assert.ThrowsException<NotFoundException>(delegate { _basket_service.GetById(updated_basket.Id); });

            foreach (var prod_id in products_indexes)
            {
                _product_service.Delete(prod_id);
                Assert.ThrowsException<NotFoundException>(delegate { _product_service.GetById(prod_id); });
            }
        }
        #endregion

        #region Private Helper Methods
        private long[] CreateProducts()
        {
            long[] products_indexes = new long[10];
            for (int i = 0; i <= 9; i++)
            {
                TestProduct.Name = TestProduct.Name + " " + i.ToString();
                var product = _product_service.Insert(TestProduct);
                products_indexes[i] = product.Id;
            }

            return products_indexes;
        }
        #endregion

        #region Private Properties

        private Basket TestBasket
        {
            get
            {
                return new Basket()
                {
                    UserId = _userId
                };
            }
        }

        private Product TestProduct
        {
            get
            {
                return new Product
                {
                    Name = "Test Product Name",
                    Description = "Test Product Description",
                    Price = 20,
                    Quantity = 1
                };
            }
        }
        #endregion

        #region Dispose
        ~BasketServiceTest()
        {
            Dispose();
        }
        private void Dispose()
        {
            _product_service.Dispose();
            _basket_service.Dispose();
        }
        #endregion
    }
}
