using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Catalog.Entities;
using Catalog.Repositories.Interfaces;
using DnsClient.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Product>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            List<Product> products = await _productRepository.GetProducts();
            return Ok(products);
        }
        
        [HttpGet]
        [Route("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Product>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            Product product = await _productRepository.GetProduct(id);
            if (product == null)
            {
                _logger.LogError($"Product with id {id} not found.");
                return NotFound();
            }
            return Ok(product);
        }
        
        [HttpGet("[Action]/{category}")]
        [ProducesResponseType(typeof(List<Product>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<List<Product>>> GetProductsByCategory(string category)
        {
            List<Product> products = await _productRepository.GetProductsByCategory(category);
            return Ok(products);
        }
        
        [HttpGet("[Action]/{name}")]
        [ProducesResponseType(typeof(List<Product>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<List<Product>>> GetProductsByName(string name)
        {
            List<Product> products = await _productRepository.GetProductsByName(name);
            return Ok(products);
        }
        
        
        [HttpPost]
        [ProducesResponseType(typeof(List<Product>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productRepository.Create(product);

            return CreatedAtRoute("GetProduct", new {id = product.Id}, product);
        }
        
        
        [HttpPut]
        [ProducesResponseType(typeof(List<Product>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _productRepository.Update(product));
        }
        
        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(List<Product>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {
            return Ok(await _productRepository.Delete(id));
        }

    }
}