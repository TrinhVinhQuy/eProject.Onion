
using WebASPMvcCore.Domain.Entities;

namespace WebASPMvcCore.Domain.Abstracts
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoryAsync();
        Task<IEnumerable<T>> GetAllCateProductCount<T>();
        Task<bool> SaveAsync(Category category);
        Task<Guid> GetCategoryIdByUrlAsync(string url);
        Task<Category> GetCategoryByIdAsync(Guid Id);
    }
}
