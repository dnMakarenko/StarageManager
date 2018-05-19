using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.WebApi.Core;
using Storage.WebApi.Services;
using Storage.WebApi.Models;
using Storage.WebApi.Exceptions;
using System;

namespace Storage.Tests
{
    [TestClass]
    public class ProductServiceTest
    {
        #region Private Fields
        private readonly IProductService _product_service = new ProductService();
        #endregion

        #region Tests
        [TestMethod]
        public void TestProductService()
        {
            var created_product = _product_service.Insert(TestProduct);
            Assert.AreNotEqual(0, created_product.Id);

            var product = _product_service.GetById(created_product.Id);
            Assert.IsNotNull(product);

            var all_products = _product_service.GetAll();
            Assert.IsNotNull(all_products);

            string product_old_name = product.Name;
            product.Name = string.Format("Updated product Name on {0}", DateTime.Now.ToShortDateString());
            var updated_product = _product_service.Update(product);
            Assert.AreNotEqual(product_old_name, updated_product.Name);

            _product_service.Delete(updated_product.Id);
            Assert.ThrowsException<NotFoundException>(delegate { _product_service.GetById(updated_product.Id); });

        }
        #endregion


        #region Private Properties

        private Product TestProduct
        {
            get {
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
        ~ProductServiceTest()
        {
            Dispose();
        }
        private void Dispose()
        {
            _product_service.Dispose();
        }
        #endregion
    }
}
