using Cohorts_First_Homework.Entities;
using Cohorts_First_Homework.Services.Interfaces;

namespace Cohorts_First_Homework.Services.Concretes
{
    public class FakeProductService : IProductService
    {
        private static List<Product> _products = new List<Product>
    {
        new Product { Id = 1, Name = "Laptop", Price = 1500, Stock = 10, CreatedAt = DateTime.UtcNow },
        new Product { Id = 2, Name = "Mouse", Price = 30, Stock = 50, CreatedAt = DateTime.UtcNow }
    };
        //
        public IEnumerable<Product> GetAll() => _products;

        public IEnumerable<Product> GetFiltered(string name, decimal? minPrice, decimal? maxPrice, int? stock, string sortBy, string order)
        {
            var result = _products.Where(p =>
                (string.IsNullOrEmpty(name) || p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) &&
                (!minPrice.HasValue || p.Price >= minPrice) &&
                (!maxPrice.HasValue || p.Price <= maxPrice) &&
                (!stock.HasValue || p.Stock >= stock));

            return sortBy.ToLower() switch
            {
                "price" => order == "desc" ? result.OrderByDescending(p => p.Price) : result.OrderBy(p => p.Price),
                "stock" => order == "desc" ? result.OrderByDescending(p => p.Stock) : result.OrderBy(p => p.Stock),
                _ => order == "desc" ? result.OrderByDescending(p => p.Name) : result.OrderBy(p => p.Name)
            };
        }

        public Product GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public Product Create(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            product.CreatedAt = DateTime.UtcNow;
            _products.Add(product);
            return product;
        }

        public void Update(int id, Product updatedProduct)
        {
            var p = GetById(id);
            if (p == null) return;
            p.Name = updatedProduct.Name;
            p.Price = updatedProduct.Price;
            p.Stock = updatedProduct.Stock;
            p.UpdatedAt = DateTime.UtcNow;
        }

        public void PartialUpdate(int id, Product updatedProduct)
        {
            var p = GetById(id);
            if (p == null) return;
            if (!string.IsNullOrEmpty(updatedProduct.Name)) p.Name = updatedProduct.Name;
            if (updatedProduct.Price > 0) p.Price = updatedProduct.Price;
            if (updatedProduct.Stock >= 0) p.Stock = updatedProduct.Stock;
            p.UpdatedAt = DateTime.UtcNow;
        }

        public void Delete(int id)
        {
            var p = GetById(id);
            if (p != null) _products.Remove(p);
        }
    }

}
