using eProject.Application.DTOs.OrderDetail;
using eProject.Domain.Entities;

namespace eProject.Application.Abstracts
{
    public interface IOrderDetailServices
    {
        Task<OrderDetail> InsertAsync(OrderDetail model);
    }
}
