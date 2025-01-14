using LaptopStore.Models.Account;
using LaptopStore.Models.Cart;
using LaptopStore.Models.Home;
using LaptopStore.Models.Reviews;
using Microsoft.EntityFrameworkCore;

namespace LaptopStore.Data
{
    public class ConnectDatabase : DbContext

    {

        public ConnectDatabase(DbContextOptions<ConnectDatabase> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> address { get; set; }

        public DbSet<Product> products { get; set; }
        public DbSet<ProductDetail> productDetails { get; set; }

        public DbSet<Order> orders { get; set; }

        public DbSet<OrderDetail> orderDetails { get; set; }

        public DbSet<Cart> carts { get; set; }

        public DbSet<OrderInfo> orderInfo { get; set; }

        public DbSet<Reviews > reviews { get; set; }

    }
}
