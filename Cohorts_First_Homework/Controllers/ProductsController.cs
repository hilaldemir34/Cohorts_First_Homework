using Cohorts_First_Homework.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Cohorts_First_Homework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private static List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1500, Stock = 10, CreatedAt = DateTime.UtcNow },
            new Product { Id = 2, Name = "Mouse", Price = 30, Stock = 50, CreatedAt = DateTime.UtcNow }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            return Ok(products);
        }

        [HttpGet("list")]
        public ActionResult<IEnumerable<Product>> GetList([FromQuery] string name, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] int? stock, [FromQuery] string sortBy = "name", [FromQuery] string order = "asc")
        {
            var filteredProducts = products.Where(p =>
                (string.IsNullOrEmpty(name) || p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) &&
                (!minPrice.HasValue || p.Price >= minPrice.Value) &&
                (!maxPrice.HasValue || p.Price <= maxPrice.Value) &&
                (!stock.HasValue || p.Stock >= stock.Value)
            );

            filteredProducts = sortBy.ToLower() switch
            {
                "price" => order.ToLower() == "desc" ? filteredProducts.OrderByDescending(p => p.Price) : filteredProducts.OrderBy(p => p.Price),
                "stock" => order.ToLower() == "desc" ? filteredProducts.OrderByDescending(p => p.Stock) : filteredProducts.OrderBy(p => p.Stock),
                _ => order.ToLower() == "desc" ? filteredProducts.OrderByDescending(p => p.Name) : filteredProducts.OrderBy(p => p.Name)
            };

            return Ok(filteredProducts.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound(new { message = "Product not found" });
            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> Create([FromBody] Product product)
        {
            if (product == null || string.IsNullOrEmpty(product.Name) || product.Price <= 0 || product.Stock < 0)
                return BadRequest(new { message = "Invalid product data" });

            product.Id = products.Count + 1;
            product.CreatedAt = DateTime.UtcNow;
            products.Add(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product updatedProduct)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound(new { message = "Product not found" });

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Stock = updatedProduct.Stock;
            product.UpdatedAt = DateTime.UtcNow;
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartialUpdate(int id, [FromBody] Product updatedProduct)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound(new { message = "Product not found" });

            if (!string.IsNullOrEmpty(updatedProduct.Name)) product.Name = updatedProduct.Name;
            if (updatedProduct.Price > 0) product.Price = updatedProduct.Price;
            if (updatedProduct.Stock >= 0) product.Stock = updatedProduct.Stock;
            product.UpdatedAt = DateTime.UtcNow;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound(new { message = "Product not found" });

            products.Remove(product);
            return NoContent();
        }
    }
}
