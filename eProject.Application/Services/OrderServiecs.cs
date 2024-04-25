using AutoMapper;
using eProject.Application.Abstracts;
using eProject.Application.DTOs.Order;
using eProject.Domain.Abstracts;
using eProject.Domain.Entities;

namespace eProject.Application.Services
{
    public class OrderServiecs : IOrderServices
    {
        private readonly IRepositoryBase<Order> _orderRepository;
        private IMapper _mapper;
        public OrderServiecs(IRepositoryBase<Order> orderRepository,IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<Order> InsertAsync(Order model)
        {
            return await _orderRepository.InsertAsync(model);
        }
    }
}
