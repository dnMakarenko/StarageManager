using Microsoft.AspNet.Identity;
using Storage.WebApi.Core;
using Storage.WebApi.Exceptions;
using Storage.WebApi.Models;
using Storage.WebApi.Services;
using Storage.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Storage.WebApi.Controllers
{
    [RoutePrefix("api/Basket")]
    [Authorize]
    public class ShoppingCartController : ApiController
    {
        #region Private Fields
        /// <summary>
        /// Service for working with Products
        /// </summary>
        private readonly IProductService _product_service;
        /// <summary>
        /// Service for working with Products basket
        /// </summary>
        private readonly IBasketService _basket_service;
        /// <summary>
        /// User Identifier
        /// </summary>
        private readonly string _userId;

        public bool IsAuthenticated
        {
            get
            {
                return IsUserAuthenticated();
            }
        }
        #endregion

        #region Init
        public ShoppingCartController(IProductService productService, IBasketService basketService)
        {
            _userId = User.Identity.GetUserId();
            _product_service = productService;
            _basket_service = basketService;
        }
        public ShoppingCartController(IBasketService basketService)
        {
            _userId = User.Identity.GetUserId();
            _basket_service = basketService;
        }
        #endregion


        #region Crud Operations
        /// <summary>
        /// Returns Shopping Cart by UserId
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                var basket = _basket_service.GetBasket(_userId);
                var basket_dto = ConvertToDto(basket);

                return Ok(basket_dto);
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(string.Format("Couldn't return Shopping Cart for UserId '{0}'", _userId), ex));
            }
        }

        /// <summary>
        /// Add Product to Shopping Cart
        /// </summary>
        /// <param name="id">Id of Product</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (productDto.Id == null)
            {
                return BadRequest();
            }

            var product = FindProduct(productDto);
            if (product == null)
            {
                return NotFound();
            }
            try
            {
                var basket = _basket_service.GetBasket(_userId);
                basket.Products.Add(new BasketProduct() { BasketId = basket.Id, ProductId = product.Id });

                _basket_service.Update(basket);

                var updated_basket = _basket_service.GetBasket(_userId);

                var basket_dto = ConvertToDto(updated_basket);

                return Ok(basket_dto);
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(string.Format("Couldn't add Product to Cart"), ex));
            }
        }

        /// <summary>
        /// Remove first product from Shopping Cart where ProductId = id
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete/{id:int}")]
        public IHttpActionResult RemoveProduct(int id)
        {
            try
            {
                var basket = _basket_service.GetBasket(_userId);
                basket = _basket_service.RemoveProduct(basket, id);

                var basket_dto = ConvertToDto(basket);

                return Ok(basket_dto);
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(string.Format("Couldn't remove Product with Id '{0}' from Shopping Cart", id), ex));
            }
        }

        /// <summary>
        /// Remove all products from the Shopping Cart where ProductId = id
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteAll/{id:int}")]
        public IHttpActionResult RemoveProducts(int id)
        {
            try
            {
                var basket = _basket_service.GetBasket(_userId);
                basket = _basket_service.RemoveProducts(basket, id);

                var basket_dto = ConvertToDto(basket);

                return Ok(basket_dto);
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(string.Format("Couldn't remove Products with Id '{0}' from Shopping Cart", id), ex));
            }
        }

        /// <summary>
        /// Remove all products from Shopping Cart
        /// </summary>
        /// <returns></returns>
        [Route("Clear")]
        [HttpGet]
        public IHttpActionResult Clear()
        {
            try {
                var basket = _basket_service.ClearBasket(_userId);
                var basket_dto = ConvertToDto(basket);

                return Ok(basket_dto);
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(string.Format("Couldn't clear Cart"), ex));
            }
        }
        #endregion

        #region Helper Dto Methods
        private BasketDto ConvertToDto(Basket basket)
        {
            var basket_dto = new BasketDto();
            if (basket != null && basket.Products != null)
            {
                var basket_products = basket.Products.Select(q => q.Product).ToList();

                foreach (var prod in basket_products)
                {
                    if (!basket_dto.Products.Where(q => q.ProductId == prod.Id.ToString()).Any())
                    {
                        ProductDto prod_dto = new ProductDto();
                        AutoMapper.Mapper.Map(prod, prod_dto);
                        basket_dto.Products.Add(new BasketProductDto() { ProductId = prod.Id.ToString(), Product = prod_dto, Quantity = basket_products.Where(q => q.Id == prod.Id).Count() });
                    }
                }
            }
            return basket_dto;
        }

        private Product FindProduct(ProductDto productDto)
        {
            long id = Convert.ToInt64(productDto.Id);
            try
            {
                var product = _product_service.GetById(id);

                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Couldn't find Product with Id '{0}'.", id));
            }
        }
        #endregion

        #region Private Helper Properties
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
        #endregion

        #region Private Helper Methods
        private bool IsUserAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Disposing
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(_basket_service != null)
                    _basket_service.Dispose();
                if (_product_service != null)
                    _product_service.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}