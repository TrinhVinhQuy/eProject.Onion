using eProject.Application.DTOs.Order;
using eProject.Domain.Entities;

namespace eProject.Application.Abstracts
{
    public interface IOrderServices
    {
        Task<Order> InsertAsync(Order model);
        Task<IEnumerable<OrderDTO>> GetAllAsync();
        Task<OrderDetailDTO> GetByIdAsync(int id);
    }
}
