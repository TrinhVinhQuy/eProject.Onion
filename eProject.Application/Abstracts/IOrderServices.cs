using eProject.Application.DTOs.Order;
using eProject.Domain.Entities;

namespace eProject.Application.Abstracts
{
    public interface IOrderServices
    {
        Task<Order> InsertAsync(Order model);
    }
}
