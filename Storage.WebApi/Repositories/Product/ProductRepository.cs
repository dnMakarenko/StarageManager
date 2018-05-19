using Storage.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Storage.WebApi.Repository
{
    public class ProductRepository : BaseRepository<Product>
    {
        #region Init
        public ProductRepository() : base(new StorageDbContext()) { }
        #endregion

        #region Crud Operations
        #endregion
    }
}