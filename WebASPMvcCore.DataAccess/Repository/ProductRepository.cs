using Dapper;
using Microsoft.EntityFrameworkCore;
using WebASPMvcCore.DataAccess.Data;
using WebASPMvcCore.Domain.Abstracts;
using WebASPMvcCore.Domain.Entities;

namespace WebASPMvcCore.DataAccess.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ISQLQueryHandler _sqLQueryHandler;
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductRepository(ApplicationDbContext applicationDbContext, ISQLQueryHandler sqLQueryHandler) : base(applicationDbContext)
        {
            _sqLQueryHandler = sqLQueryHandler;
            _applicationDbContext = applicationDbContext;
        }
        public async Task<(IEnumerable<T>, int)> GetProductsByPaginationAsync<T>(int pageIndex,
            int pageSize, string? keyword, Guid? categoryId, decimal? priceMin, decimal? priceMax)
        {
            DynamicParameters parammeters = new DynamicParameters();

            parammeters.Add("keyword", keyword, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parammeters.Add("categoryId", categoryId, System.Data.DbType.Guid, System.Data.ParameterDirection.Input);
            parammeters.Add("priceMin", priceMin, System.Data.DbType.Decimal, System.Data.ParameterDirection.Input);
            parammeters.Add("priceMax", priceMax, System.Data.DbType.Decimal, System.Data.ParameterDirection.Input);
            parammeters.Add("pageIndex", pageIndex, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            parammeters.Add("pageSize", pageSize, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            parammeters.Add("totalRecords", 0, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            var result = await _sqLQueryHandler.ExecuteStoreProdecureReturnListAsync<T>("GetALLProductByPagination", parammeters);

            var totalRecords = parammeters.Get<int>("totalRecords");

            return (result, totalRecords);
        }


        public async Task<IEnumerable<Product>> GetProductsByListCodeAsync()
        {
            return await base.GetAllAsync(x => x.IsActive == true);
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await base.GetAllAsync(null);
        }

        public async Task<Product> GetProductByUrlAsync(string url)
        {
            return await _applicationDbContext.Products.Include(p => p.ProductImages).AsNoTracking().FirstOrDefaultAsync(x => x.MetaLink == url);
        }

        public async Task<Product> GetProductByIdAsync(Guid Id)
        {
            return await base.GetSingleAsync(x => x.Id == Id);
        }

        public async Task<bool> SaveAsync(Product product)
        {
            try
            {
                var check = await base.GetSingleAsync(x => x.Id == product.Id);

                if (check is null)
                {
                    product.Id = Guid.NewGuid();
                    await base.CreateAsync(product);
                }
                else
                {
                    base.Update(product);
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
