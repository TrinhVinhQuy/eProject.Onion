using WebASPMvcCore.Domain.Entities;

namespace WebASPMvcCore.Domain.Abstracts
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetOrderDetailByOrderIdAsync(Guid Id);
    }
}
