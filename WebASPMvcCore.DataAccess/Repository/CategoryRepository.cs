using Dapper;
using WebASPMvcCore.DataAccess.Data;
using WebASPMvcCore.Domain.Abstracts;
using WebASPMvcCore.Domain.Entities;

namespace WebASPMvcCore.DataAccess.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private ISQLQueryHandler _sqLQueryHandler;
        public CategoryRepository(ApplicationDbContext applicationDbContext,
            ISQLQueryHandler sQLQueryHandler) : base(applicationDbContext)
        {
            _sqLQueryHandler = sQLQueryHandler;
        }

        public async Task<List<Category>> GetAllCategoryAsync()
        {
            return await GetAllAsync(null);
        }

        public async Task<IEnumerable<T>> GetAllCateProductCount<T>()
        {
            DynamicParameters parameters = new DynamicParameters();

            var sqlQuery = @"
                            SELECT 
                                c.Id, 
                                c.Name, 
                                c.MetaDescription, 
                                c.MetaImage, 
                                c.MetaKeywords, 
                                c.MetaLink, 
                                c.MetaTitle, 
                                COUNT(p.id) AS ProductTotal
                            FROM 
                                Categories c 
                            LEFT JOIN 
                                Products p 
                            ON 
                                c.id = p.CategoryId 
                            GROUP BY 
                                c.Id, 
                                c.Name, 
                                c.MetaDescription, 
                                c.MetaImage, 
                                c.MetaKeywords, 
                                c.MetaLink, 
                                c.MetaTitle;";

            var result = await _sqLQueryHandler.ExecuteReturnListRowAsync<T>(sqlQuery, parameters);

            return result;
        }

        public async Task<Guid> GetCategoryIdByUrlAsync(string url)
        {
            var categoryId = await base.GetSingleAsync(x => x.MetaLink == url && x.IsActive == true);
            return categoryId.Id;
        }
        public async Task<Category> GetCategoryByIdAsync(Guid Id)
        {
            return await base.GetSingleAsync(x => x.Id == Id && x.IsActive == true);
        }

        public async Task<bool> SaveAsync(Category category)
        {
            try
            {
                if (category.Id == Guid.Empty)
                {
                    await base.CreateAsync(category);
                }
                else
                {
                    base.Update(category);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
