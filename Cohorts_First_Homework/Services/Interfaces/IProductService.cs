using Cohorts_First_Homework.Entities;

namespace Cohorts_First_Homework.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetFiltered(string name, decimal? minPrice, decimal? maxPrice, int? stock, string sortBy, string order);
        Product GetById(int id);
        Product Create(Product product);
        void Update(int id, Product updatedProduct);
        void PartialUpdate(int id, Product updatedProduct);
        void Delete(int id);
    }
}
