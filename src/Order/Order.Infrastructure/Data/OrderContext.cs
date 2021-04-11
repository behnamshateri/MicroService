using Microsoft.EntityFrameworkCore;

namespace Order.Infrastructure.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options): base(options)
        {
            
        }

        public DbSet<Basket.Entities.Order> Orders { get; set; }
    }
}