using Dapper;
using WebASPMvcCore.DataAccess.Data;
using WebASPMvcCore.Domain.Abstracts;
using WebASPMvcCore.Domain.Entities;

namespace WebASPMvcCore.DataAccess.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ISQLQueryHandler _sqLQueryHandler;
        public OrderRepository(ApplicationDbContext applicationDbContext, ISQLQueryHandler sqLQueryHandler) : base(applicationDbContext)
        {
            _sqLQueryHandler = sqLQueryHandler;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var result = await _sqLQueryHandler.ExecuteStoreProdecureReturnListAsync<T>("GetAllOrder", null);
            return result;
        }

        public async Task<Order> GetOrderAsync(Guid Id)
        {
            return await base.GetSingleAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<T>> GetOrderByIdAsync<T>(Guid Id)
        {
            DynamicParameters parammeters = new DynamicParameters();

            parammeters.Add("OrderId", Id, System.Data.DbType.Guid, System.Data.ParameterDirection.Input);
            var result = await _sqLQueryHandler.ExecuteReturnListRowAsync<T>("GetOrderDetailsById", parammeters);
            return result;
        }

        public async Task SaveAsync(Order order)
        {
            await base.CreateAsync(order);
        }
    }
}
