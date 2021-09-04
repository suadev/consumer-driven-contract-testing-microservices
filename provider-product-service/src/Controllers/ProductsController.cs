using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Dto;

namespace Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDBContext _dbContext;
        public ProductsController(ProductDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            var products = await _dbContext.Products
                .Select(s => new ProductDto
                {
                    ID = s.ID,
                    IsActive = s.IsActive,
                    Name = s.Name,
                    StockCount = s.StockCount
                }).ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _dbContext.Products
                .Select(s => new ProductDto
                {
                    ID = s.ID,
                    IsActive = s.IsActive,
                    Name = s.Name,
                    StockCount = s.StockCount
                }).FirstOrDefaultAsync(s => s.ID == id);
            return Ok(product);
        }

        [HttpGet("search")]
        public async Task<ActionResult> Search([FromQuery] string keyword)
        {
            var normalizedKeyword = keyword.ToUpperInvariant();

            var products = await _dbContext.Products
                .Where(s => s.NormalizedName.Contains(keyword.ToUpperInvariant()))
                .Select(s => new ProductDto
                {
                    ID = s.ID,
                    IsActive = s.IsActive,
                    Name = s.Name,
                    StockCount = s.StockCount
                }).ToListAsync();
            return Ok(products);
        }
    }
}
