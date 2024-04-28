using AutoMapper;
using eProject.Application.Abstracts;
using eProject.Application.DTOs.Order;
using eProject.Application.DTOs.Product;
using eProject.Domain.Abstracts;
using eProject.Domain.Entities;
using System.Net.WebSockets;

namespace eProject.Application.Services
{
    public class OrderServiecs : IOrderServices
    {
        private readonly IRepositoryBase<Order> _orderRepository;
        private readonly IRepositoryBase<OrderDetail> _orderDetailRepository;
        private readonly IRepositoryBase<Product> _productDetailRepository;
        private readonly IRepositoryBase<User> _userRepository;
        private IMapper _mapper;
        public OrderServiecs(IRepositoryBase<Order> orderRepository,
            IMapper mapper,
            IRepositoryBase<User> userRepository,
            IRepositoryBase<OrderDetail> orderDetailRepository,
            IRepositoryBase<Product> productDetailRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _orderDetailRepository = orderDetailRepository;
            _productDetailRepository = productDetailRepository;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            var _orders = await _orderRepository.GetAllAsync();
            if (_orders.Count() > 0)
            {
                var orders = _mapper.Map<IEnumerable<OrderDTO>>(_orders);
                foreach (var order in orders)
                {
                    var user = await _userRepository.GetByIdAsync(order.UserId);
                    order.Name = user.Name;
                }
                return orders;
            }
            return null;
        }

        public async Task<OrderDetailDTO> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order != null)
            {
                var orderDetail = _mapper.Map<OrderDetailDTO>(order);
                var user = await _userRepository.GetByIdAsync(order.UserId);
                orderDetail = _mapper.Map(user, orderDetail);

                //if (order.StaffId != null)
                //{
                //    var nameStaff = await _userRepository.GetByIdAsync(order.UserId);
                //    orderDetail.NameStaff = nameStaff.Name;
                //}

                var _orderDetails = await _orderDetailRepository.GetAllAsync();
                _orderDetails = _orderDetails.Where(x => x.OrderId == id);
                var lisrIdProduct = _orderDetails.Select(x => x.ProductId);

                var products = await _productDetailRepository.GetAllAsync();

                orderDetail.Products = _mapper.Map<List<ProductCart>>(products.Where(x => lisrIdProduct.Contains(x.Id)));

                foreach (var item in orderDetail.Products)
                {
                    item.Quantity = _orderDetails.First(x => x.ProductId == item.Id).Quanlity;
                    item.Price = _orderDetails.First(x => x.ProductId == item.Id).Price;
                    item.Discount = 0;
                }

                return orderDetail;
            }
            return null;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<Order> InsertAsync(Order model)
        {
            return await _orderRepository.InsertAsync(model);
        }

        public async Task UpdateAsync(Order entity)
        {
            await _orderRepository.UpdateAsync(entity);
        }
    }
}
