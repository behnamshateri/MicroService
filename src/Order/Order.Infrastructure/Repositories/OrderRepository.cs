using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Order.Core.Repositories;
using Order.Infrastructure.Data;
using Order.Infrastructure.Repositories.Base;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Basket.Entities.Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<IEnumerable<Basket.Entities.Order>> GetOrderByUserName(string userName)
        {
            return await _dbContext.Orders.Where(x => x.UserName == userName).ToListAsync();
        }
    }
}