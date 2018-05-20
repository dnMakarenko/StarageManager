using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Data.Entity;

namespace Storage.WebApi.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BasketId { get; set; }

        [NotMapped]
        public virtual Basket UserBasket
        {
            get
            {
                using (var db = new StorageDbContext())
                {
                    var basket = db.Baskets.Find(BasketId);
                    if (basket == null)
                    {
                        return new Basket()
                        {
                            UserId = Id,
                            Products = new List<BasketProduct>()
                        };
                    }
                    return basket;
                }
            }
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var user = new ApplicationUser()
            {
                UserName = "aspmvcrazor@gmail.com",
                Email = "aspmvcrazor@gmail.com",
                EmailConfirmed = true,
                FirstName = "Denis",
                LastName = "Makarenko"
            };

            manager.Create(user, "123456_Aa");

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByName("aspmvcrazor@gmail.com");

            manager.AddToRole(adminUser.Id, "Admin");
            base.Seed(context);
        }
    }
}