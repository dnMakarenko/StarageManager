using Storage.WebApi.Models;
using Storage.WebApi.Services;

namespace Storage.WebApi.Core
{
    public interface IBasketService : IService<Basket>
    {
        Basket ClearBasket(string userId);
        /// <summary>
        /// Get User`s Shopping Cart by UserId
        /// </summary>
        /// <param name="userId">Id of User</param>
        /// <returns>ShoppingCart</returns>
        Basket GetBasket(string userId);
        /// <summary>
        /// Create Shopping Cart for User if not exist
        /// </summary>
        /// <param name="userId">Id of User</param>
        /// <returns>ShoppingCart</returns>
        Basket CreateBasket(string userId);
        /// <summary>
        /// Add Product To Cart
        /// </summary>
        /// <param name="entity">Cart</param>
        /// <param name="productId">Product Id</param>
        /// <returns>ShoppingCart</returns>
        Basket AddProduct(Basket entity, long productId);
        /// <summary>
        /// Remove first product from the Cart where product.Id = productId
        /// </summary>
        /// <param name="entity">Cart</param>
        /// <param name="productId">Product Id</param>
        /// <returns>ShoppingCart</returns>
        Basket RemoveProduct(Basket entity, long productId);
        /// <summary>
        /// Remove all products from the Cart where product.Id = productId 
        /// </summary>
        /// <param name="entity">Cart</param>
        /// <param name="productId">Product Id</param>
        /// <returns>ShoppingCart</returns>
        Basket RemoveProducts(Basket entity, long productId);
    }
}