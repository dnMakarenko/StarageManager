using Storage.WebApi.Models;
using Storage.WebApi.Repository;

namespace Storage.WebApi.Repository
{
    public class BasketProductRepository : BaseRepository<BasketProduct>
    {
        #region Init
        public BasketProductRepository() : base(new StorageDbContext()) { }
        #endregion
    }
}