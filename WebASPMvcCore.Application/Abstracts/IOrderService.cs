using WebASPMvcCore.Application.DTOs.Order;
using WebASPMvcCore.Domain.Enums;

namespace WebASPMvcCore.Application.Abstracts
{
    public interface IOrderService
    {
        Task<bool> SaveAsync(OrderDTO order);
        Task<IEnumerable<OrderShowDTO>> GetAllOrderAsync();
        Task<IEnumerable<OrderShowDetailDTO>> GetOrderDetailByIdAsync(Guid Id);
        Task<bool> UpdateOrderStatusProcessingAsync(Guid Id, StatusProcessing StatusProcessing);
    }
}
