using AutoMapper;
using eProject.Application.Abstracts;
using eProject.Domain.Abstracts;
using eProject.Domain.Entities;

namespace eProject.Application.Services
{
    public class OrderDetailServices : IOrderDetailServices
    {
        private readonly IRepositoryBase<OrderDetail> _orderDetailRepository;
        private readonly IMapper _mapper;
        public OrderDetailServices(IRepositoryBase<OrderDetail> orderDetailRepository, IMapper mapper)
        {
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
        }
        public async Task<OrderDetail> InsertAsync(OrderDetail model)
        {
            return await _orderDetailRepository.InsertAsync(model);
        }
    }
}
