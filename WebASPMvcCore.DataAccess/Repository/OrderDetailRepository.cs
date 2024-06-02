using WebASPMvcCore.DataAccess.Data;
using WebASPMvcCore.Domain.Abstracts;
using WebASPMvcCore.Domain.Entities;

namespace WebASPMvcCore.DataAccess.Repository
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private ISQLQueryHandler _sqLQueryHandler;
        public OrderDetailRepository(ApplicationDbContext applicationDbContext,
            ISQLQueryHandler sQLQueryHandler) : base(applicationDbContext)
        {
            _sqLQueryHandler = sQLQueryHandler;
        }
        public async Task<IEnumerable<OrderDetail>> GetOrderDetailByOrderIdAsync(Guid Id)
        {
            return await base.GetAllAsync(x => x.OrderId == Id);
        }
    }
}
