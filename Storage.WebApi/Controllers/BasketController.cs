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
    public class BasketController : ApiController
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
        public BasketController()
        {
            _userId = User.Identity.GetUserId();
            _product_service = new ProductService();
            _basket_service = new BasketService();
        }
        #endregion


        #region Crud Operations
        /// <summary>
        /// Get user`s basket by UserId
        /// </summary>
        /// <returns></returns>
        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            if (!IsAuthenticated)
            {
                return StatusCode(System.Net.HttpStatusCode.Unauthorized);
            }
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
                return InternalServerError(new Exception(string.Format("Couldn't return Basket for UserId '{0}'", _userId), ex));
            }
        }

        /// <summary>
        /// Add product to  basket
        /// </summary>
        /// <param name="id">Id of Product</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddProduct/{id}")]
        public IHttpActionResult AddProduct(int id)
        {
            if (!IsAuthenticated)
            {
                return Unauthorized();
            }
            var prod = _product_service.GetById(id);
            try
            {
                var basket = _basket_service.GetBasket(_userId);
                basket.Products.Add(new BasketProduct() { BasketId = basket.Id, ProductId = prod.Id });
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
                return InternalServerError(new Exception(string.Format("Couldn't add Product to Basket"), ex));
            }
        }

        /// <summary>
        /// Remove product by product Id
        /// </summary>
        /// <param name="id">Id of product</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("RemoveProduct/{id}")]
        public IHttpActionResult RemoveProduct(int id)
        {
            if (!IsAuthenticated)
            {
                return Unauthorized();
            }
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
                return InternalServerError(new Exception(string.Format("Couldn't remove Product from Basket"), ex));
            }
        }

        /// <summary>
        /// Remove all products from basket
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Clear")]
        public IHttpActionResult Clear()
        {
            if (!IsAuthenticated)
            {
                return Unauthorized();
            }
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
                return InternalServerError(new Exception(string.Format("Couldn't clear Basket"), ex));
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