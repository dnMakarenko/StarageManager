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
        private readonly IProductService _productService;
        #endregion

        #region Init
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion


        #region Crud Operations
        /// <summary>
        /// Returns All Products from database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetProducts()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                if (products == null || products.Count == 0)
                {
                    products = CreateProducts();
                }
                var productsDto = new List<ProductDto>();

                AutoMapper.Mapper.Map(products, productsDto);

                return Ok(productsDto);
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
        [Route("ById/{id:int}")]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                var productDto = new ProductDto();

                AutoMapper.Mapper.Map(product, productDto);

                return Ok(productDto);
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
        public async Task<IHttpActionResult> PostProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var product = new Product();
                AutoMapper.Mapper.Map(productDto, product);

                _productService.Insert(product);
                productDto.Id = product.Id.ToString();

                return CreatedAtRoute("DefaultApi", new { id = productDto.Id }, productDto);
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
        public async Task<IHttpActionResult> PutProduct(long id, ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id.ToString() != productDto.Id)
            {
                return BadRequest();
            }

            try
            {
                productDto.Id = id.ToString();

                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                AutoMapper.Mapper.Map(productDto, product);

                _productService.Update(product);

                return Ok(productDto);
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
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);

                _productService.Delete(id);

                var productDto = new ProductDto();
                AutoMapper.Mapper.Map(product, productDto);
                return Ok(productDto);
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
       /* [HttpPost]
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
        }*/
        #endregion

        #region Private Helper Methods
        private List<Product> CreateProducts()
        {
            var products = new List<Product>();
            for (int i = 0; i <= 9; i++)
            {
                TestProduct.Name = TestProduct.Name + " " + i.ToString();
                var product = _productService.Insert(TestProduct);
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
                if (_productService != null)
                    _productService.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}