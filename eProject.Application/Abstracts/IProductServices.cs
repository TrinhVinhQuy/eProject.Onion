using eProject.Application.DTOs.Product;
using eProject.Domain.Entities;

namespace eProject.Application.Abstracts
{
    public interface IProductServices
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<IEnumerable<ProductCategoryDTO>> GetByCategoryAsync();
        Task<ProductDTO> GetByIdAsync(int id);
    }
}
