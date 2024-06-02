using WebASPMvcCore.Application.DTOs.Category;

namespace WebASPMvcCore.Application.Abstracts
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllAsync();
        Task<List<CategoryProductCountDTO>> GetAllCateProductCount();
        Task<Guid> GetCategoryIdByUrlAsync(string url);
    }
}
