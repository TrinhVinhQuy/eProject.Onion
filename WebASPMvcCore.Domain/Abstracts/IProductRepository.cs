using WebASPMvcCore.Domain.Entities;

namespace WebASPMvcCore.Domain.Abstracts
{
    public interface IProductRepository
    {
        Task<(IEnumerable<T>, int)> GetProductsByPaginationAsync<T>(int pageIndex, int pageSize, string? keyword, 
            Guid? categoryId, decimal? priceMin, decimal? priceMax);
        Task<IEnumerable<Product>> GetProductsByListCodeAsync();
        Task<Product> GetProductByUrlAsync(string url);
        Task<Product> GetProductByIdAsync(Guid Id);
        Task<bool> SaveAsync(Product product);
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}
