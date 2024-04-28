using eProject.Application.DTOs.Product;
using eProject.Domain.Entities;

namespace eProject.Application.Abstracts
{
    public interface IProductServices
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<IEnumerable<ProductCategoryDTO>> GetByCategoryAsync();
        Task<ProductDTO> GetByIdAsync(int id);
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> InsertAsync(Product entity);
        Task UpdateAsync(Product entity);
    }
}
