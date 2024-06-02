using AutoMapper;
using System.Linq;
using WebASPMvcCore.Application.Abstracts;
using WebASPMvcCore.Application.DTOs.Order;
using WebASPMvcCore.Domain.Abstracts;
using WebASPMvcCore.Domain.Entities;
using WebASPMvcCore.Domain.Enums;

namespace WebASPMvcCore.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderShowDTO>> GetAllOrderAsync()
        {
            var order = await _unitOfWork.OrderRepository.GetAllAsync<OrderShowDTO>();
            return _mapper.Map<IEnumerable<OrderShowDTO>>(order);
        }

        public async Task<IEnumerable<OrderShowDetailDTO>> GetOrderDetailByIdAsync(Guid Id)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderByIdAsync<OrderShowDetailDTO>(Id);
            return _mapper.Map<IEnumerable<OrderShowDetailDTO>>(order);
        }

        public async Task<bool> SaveAsync(OrderDTO orderDTO)
        {
            try
            {
                var order = _mapper.Map<Order>(orderDTO);

                var _productList = await _unitOfWork.ProductRepository.GetProductsByListCodeAsync();
                _productList = _productList.Where(x => orderDTO.Items.Select(x => x.ProductId).Contains(x.Id));

                await _unitOfWork.BeginTransaction();

                await _unitOfWork.OrderRepository.SaveAsync(order);

                await _unitOfWork.SaveChangeAsync();

                if (orderDTO.Items.Any())
                {
                    foreach (var product in orderDTO.Items)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.Id,
                            ProductId = product.ProductId,
                            Quanlity = product.Quanlity,
                            UnitPrice = product.UnitPrice,
                        };
                        var productEntity = _productList.First(x => x.Id == product.ProductId);
                        productEntity.Quantity -= product.Quanlity;

                        await _unitOfWork.Table<OrderDetail>().AddAsync(orderDetail);
                        _unitOfWork.Table<Product>().Update(productEntity);
                    }
                    await _unitOfWork.SaveChangeAsync();
                }


                await _unitOfWork.CommitTransactionAsync();

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateOrderStatusProcessingAsync(Guid Id, StatusProcessing StatusProcessing)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderAsync(Id);
            if (order == null)
            {
                return false;
            }
            try
            {
                await _unitOfWork.BeginTransaction();

                var orderDetail = await _unitOfWork.OrderDetailRepository.GetOrderDetailByOrderIdAsync(Id);
                var _productList = await _unitOfWork.ProductRepository.GetProductsByListCodeAsync();

                if (StatusProcessing == (Domain.Enums.StatusProcessing)3)
                {
                    foreach (var _orderDetail in orderDetail)
                    {
                        var productEntity = _productList.First(x => x.Id == _orderDetail.ProductId);
                        productEntity.Quantity += _orderDetail.Quanlity;
                        _unitOfWork.Table<Product>().Update(productEntity);
                    }
                }

                if(StatusProcessing == (Domain.Enums.StatusProcessing)4)
                {
                    foreach (var _orderDetail in orderDetail)
                    {
                        var productEntity = _productList.First(x => x.Id == _orderDetail.ProductId);
                        productEntity.SoldItem += _orderDetail.Quanlity;
                        _unitOfWork.Table<Product>().Update(productEntity);
                    }
                }

                order.StatusProcessing = StatusProcessing;
                _unitOfWork.Table<Order>().Update(order);

                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
            return true;
        }
    }
}
