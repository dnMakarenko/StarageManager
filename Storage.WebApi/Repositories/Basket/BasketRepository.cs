using System;
using System.Collections.Generic;
using Storage.WebApi.Models;
using System.Linq;
using System.Threading.Tasks;
using Storage.WebApi.Exceptions;
using System.Data.Entity;

namespace Storage.WebApi.Repository
{
    public class BasketRepository : BaseRepository<Basket>
    {
        #region Init
        public BasketRepository() : base(new StorageDbContext()) { }
        #endregion

        #region Cruds
        public override List<Basket> GetAll()
        {
            try
            {
                var baskets = base.GetAll();
                return baskets;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Couldn't return Baskets. Error: '{0}'.", ex.Message), ex);
            }
        }
        public override async Task<List<Basket>> GetAllAsync()
        {
            try
            {
                var baskets = await base.GetAllAsync();
                return baskets;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Couldn't return baskets. Error: '{0}'.", ex.Message), ex);
            }
        }

        public override Basket GetById(object Id)
        {
            try
            {
                long id = Convert.ToInt64(Id);
                var basket = base.dbSet.Include(x => x.Products.Select(q => q.Product)).Where(q => q.Id == id).FirstOrDefault();
                if (basket == null)
                {
                    throw new NotFoundException(string.Format("Basket with Id '{0}' not found!", Id));
                }
                return basket;
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Couldn't return Basket with Id '{0}'. Error: '{1}'.", Id, ex.Message), ex);
            }
        }
        public override async Task<Basket> GetByIdAsync(object Id)
        {
            try
            {
                var basket = await base.GetByIdAsync(Id);
                if (basket == null)
                {
                    throw new NotFoundException(string.Format("Basket with Id '{0}' not found!", Id));
                }
                return basket;
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Couldn't return Basket with Id '{0}'. Error: '{1}'.", Id, ex.Message), ex);
            }
        }

        public override Basket Insert(Basket entity)
        {
            try
            {
                return base.Insert(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Couldn't add Basket. Error: '{0}'.", ex.Message), ex);
            }
        }

        public override void Delete(object Id)
        {
            try
            {
                base.Delete(Id);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Couldn't delete Basket with Id '{0}'.", Id), ex);
            }

        }
        public override void Delete(Basket basket)
        {
            try
            {
                base.Delete(basket);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Couldn't delete Basket with Id '{0}'.", basket.Id), ex);
            }
        }

        public override Basket Update(Basket entity)
        {
            try
            {
                return base.Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Couldn't update Basket with Id '{0}'. Error: '{1}'.", entity.Id, ex.Message), ex);
            }
        }

        public override void SaveChanges()
        {
            try
            {
                base.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't update database. See inner exception!", ex);
            }
        }

        public override async Task SaveChangesAsync()
        {
            try
            {
                await base.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't update database. See inner exception!", ex);
            }
        }
        #endregion
    }
}