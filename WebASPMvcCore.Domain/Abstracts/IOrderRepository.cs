using WebASPMvcCore.Domain.Entities;

namespace WebASPMvcCore.Domain.Abstracts
{
    public interface IOrderRepository
    {
        Task SaveAsync(Order order);
        Task<IEnumerable<T>> GetAllAsync<T>();
        Task<IEnumerable<T>> GetOrderByIdAsync<T>(Guid Id);
        Task<Order> GetOrderAsync(Guid Id);
    }
}
