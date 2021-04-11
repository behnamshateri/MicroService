using StackExchange.Redis;

namespace Basket.Data
{
    public interface IBasketContext
    {
        IDatabase Redis { get; }
    }
}