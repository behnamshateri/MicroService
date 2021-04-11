using StackExchange.Redis;

namespace Basket.Data
{
    public class BasketContext : IBasketContext
    {
        public IDatabase Redis { get; }   // khode Db hastesh
        private readonly ConnectionMultiplexer _redisConnection;  // connection redis hast

        public BasketContext(ConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            Redis = redisConnection.GetDatabase();
        }
    }
}