using Microsoft.AspNetCore.Http;
using WebASPMvcCore.Application.DTOs;
using WebASPMvcCore.Application.DTOs.Product;
using WebASPMvcCore.Domain.Entities;

namespace WebASPMvcCore.Application.Abstracts
{
    public interface IProductService
    {
        Task<ResponseDatatable<ProductDTO>> GetBooksByPaginationAsync(RequestDatatable request);
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<IEnumerable<ProductDTO>> GetAllProductAsync();
        Task<IEnumerable<ProductDTO>> GetProductTopAsync();
        Task<ProductPriceDTO> GetProductPriceMinMaxAsync();
        Task<ProductDetailDTO> GetProductDetailAsync(string url);
        Task<ProductDetailDTO> GetProductDetailByIdAsync(Guid Id);
        Task<IEnumerable<ProductCartDTO>> GetAllProductCartsAsync();
        Task<ProductCartDTO> GetProductCartByIdAsync(Guid Id);
        Task<bool> CreateAsync(Product product, List<IFormFile> Images);
        Task<bool> IsActiveAsync(Guid Id, bool IsActive);
        Task<bool> GetProductUpdateAsync(ProductUpdateDTO product);
    }
}
