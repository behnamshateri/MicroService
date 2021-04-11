using Catalog.Entities;
using Catalog.Settings;
using MongoDB.Driver;

namespace Catalog.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(ICatalogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Products = database.GetCollection<Product>(settings.CollectionName);

            CatalogContextSeeder.Seed(Products);
        }
        public IMongoCollection<Product> Products { get; }
    }
}