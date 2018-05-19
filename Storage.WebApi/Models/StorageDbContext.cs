using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Storage.WebApi.Models
{
    public class StorageDbContext : DbContext
    {
        #region Init
        public StorageDbContext():base("StorageDbConnection")
        {
            //Database.SetInitializer<StorageDbContext>(new DropCreateDatabaseAlways<StorageDbContext>());
            //Database.SetInitializer<StorageDbContext>(new DropCreateDatabaseIfModelChanges<StorageDbContext>());
        }
        #endregion

        #region DbSets
        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketProduct> BasketProducts { get; set; }
        #endregion
    }
}