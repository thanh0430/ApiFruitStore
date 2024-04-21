using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiFruitStore.Data
{
    public class FruitStoreContext : IdentityDbContext<ApplicationUser>
    {
        public FruitStoreContext ( DbContextOptions <FruitStoreContext> opt) : base(opt)
        {

        }
        #region
        public DbSet<Products>? Products { get; set; }
        public DbSet<ProductCategories>? ProductCategories { get; set; }
        public DbSet<Customers>? Customers { get; set; }
        public DbSet<Orders>? Orders { get; set; }
        public DbSet<OrderDetails>?  OrderDetails { get; set; }
        public DbSet<Warehouse>? Warehouse { get; set; }


        #endregion
    }
}
