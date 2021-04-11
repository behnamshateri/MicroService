using System.Collections.Generic;
using System.Threading.Tasks;
using Order.Core.Repositories.Base;

namespace Order.Core.Repositories
{
    public interface IOrderRepository : IRepository<Basket.Entities.Order>
    {
        Task<IEnumerable<Basket.Entities.Order>> GetOrderByUserName(string userName);
    }
}