using Cohorts_First_Homework.Entities;
using Cohorts_First_Homework.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Cohorts_First_Homework.Controllers
{
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Get() => Ok(_productService.GetAll());

        [HttpGet("list")]
        public IActionResult GetList(string name, decimal? minPrice, decimal? maxPrice, int? stock, string sortBy = "name", string order = "asc")
            => Ok(_productService.GetFiltered(name, minPrice, maxPrice, stock, sortBy, order));

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetById(id);
            return product == null ? NotFound(new { message = "Product not found" }) : Ok(product);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (product == null || string.IsNullOrEmpty(product.Name) || product.Price <= 0 || product.Stock < 0)
                return BadRequest(new { message = "Invalid product data" });

            var created = _productService.Create(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Product updatedProduct)
        {
            if (_productService.GetById(id) == null)
                return NotFound(new { message = "Product not found" });

            _productService.Update(id, updatedProduct);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartialUpdate(int id, Product updatedProduct)
        {
            if (_productService.GetById(id) == null)
                return NotFound(new { message = "Product not found" });

            _productService.PartialUpdate(id, updatedProduct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_productService.GetById(id) == null)
                return NotFound(new { message = "Product not found" });

            _productService.Delete(id);
            return NoContent();
        }
    }

}
