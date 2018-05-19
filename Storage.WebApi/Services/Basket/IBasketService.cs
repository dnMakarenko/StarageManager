using Storage.WebApi.Models;
using Storage.WebApi.Services;

namespace Storage.WebApi.Core
{
    public interface IBasketService : IService<Basket>
    {
        Basket ClearBasket(string userId);
        Basket GetBasket(string userId);
        Basket CreateBasket(string userId);
        Basket AddProduct(Basket entity, long productId);
        Basket RemoveProduct(Basket entity, long productId);
    }
}