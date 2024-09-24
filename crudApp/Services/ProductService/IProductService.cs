using crudApp.Persistence.Models;
using crudApp.Services.ProductService.DTOs;

namespace crudApp.Services.ProductService
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product CreateProduct(ProductRequestDTO request);
        Product UpdateProduct(int id, ProductRequestDTO request);
        bool DeleteProduct(int id);
    }
}
