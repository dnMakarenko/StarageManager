using Microsoft.AspNet.Identity;
using Storage.WebApi.Core;
using Storage.WebApi.Exceptions;
using Storage.WebApi.Models;
using Storage.WebApi.Services;
using Storage.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace Storage.WebApi.Controllers
{
    [RoutePrefix("api/Products")]
    [AllowAnonymous]
    public class ProductsController : ApiController
    {
        #region Private Fields
        /// <summary>
        /// Service for working with Products
        /// </summary>
        private readonly IProductService _product_service;
        #endregion

        #region Init
        public ProductsController()
        {
            _product_service = new ProductService();
        }
        #endregion


        #region Crud Operations
        /// <summary>
        /// Returns All Products from database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var products = await _product_service.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(string.Format("Couldn't return Products. Error: {0}", ex.Message), ex));
            }
        }

        /// <summary>
        /// Find product by Id
        /// </summary>
        /// <param name="productId">Id of Product</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            try
            {
                var product = await _product_service.GetByIdAsync(id);
                return Ok(product);
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(string.Format("Couldn't find Product. Error: {0}", ex.Message), ex));
            }
        }

        /// <summary>
        /// Create new Product
        /// </summary>
        /// <param name="productDto">Dto object with product data</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create(ProductDto productDto)
        {
            try
            {
                var product = new Product()
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = Convert.ToDecimal(productDto.Price)
                };

                _product_service.Insert(product);

                var response = CreatedAtRoute("DefaultApi", new { controller = "Products", action = "GetById", id = product.Id }, product);
                return response;
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(string.Format("Couldn't create Product. Error: {0}", ex.Message), ex));
            }
        }

        /// <summary>
        /// Update exiting product
        /// </summary>
        /// <param name="productDto">Dto object with product data</param>
        /// <returns></returns>
        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IHttpActionResult> Update(long id, ProductDto productDto)
        {
            try
            {
                var product = await _product_service.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.Price = Convert.ToDecimal(productDto.Price);

                _product_service.Update(product);

                return Ok(product);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(string.Format("Couldn't update Product. Error: {0}", ex.Message), ex));
            }
        }

        /// <summary>
        /// Remove product from database by Id
        /// </summary>
        /// <param name="productId">Id of exiting product</param>
        /// <returns></returns>
        [Route("Delete/{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                _product_service.Delete(id);
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(string.Format("Couldn't delete Product. Error: {0}", ex.Message), ex));
            }
        }

        /// <summary>
        /// Api for Quickly Creating some products to start test server :)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateProductsTest")]
        public IHttpActionResult CreateProductsTest()
        {
            try
            {
                var products = CreateProducts();
                return Ok(products);
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(string.Format("Couldn't create products"), ex));
            }
        }
        #endregion

        #region Private Helper Methods
        private List<Product> CreateProducts()
        {
            var products = new List<Product>();
            for (int i = 0; i <= 9; i++)
            {
                TestProduct.Name = TestProduct.Name + " " + i.ToString();
                var product = _product_service.Insert(TestProduct);
                products.Add(product);
            }

            return products;
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

        #region Disposing
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_product_service != null)
                    _product_service.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}