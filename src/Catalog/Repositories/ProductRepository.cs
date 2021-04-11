using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Data;
using Catalog.Entities;
using Catalog.Repositories.Interfaces;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }
        
        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.Find(x => true).ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetProductsByName(string name)
        {
            // FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Name, name);
            
            List<Product> products = await _context.Products.Find(x => x.Name.Contains(name)).ToListAsync();

            return products;
        }

        public async Task<List<Product>> GetProductsByCategory(string category)
        {
            // FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, category);
            
            List<Product> products = await _context.Products.Find(x => x.Category.Contains(category)).ToListAsync();
            
            return products;
        }

        public async Task Create(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> Update(Product product)
        {
            ReplaceOneResult result = await _context.Products.ReplaceOneAsync(x => x.Id == product.Id, product);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            DeleteResult result = await _context.Products.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}