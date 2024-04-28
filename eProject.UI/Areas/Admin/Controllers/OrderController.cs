using eProject.Application.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly IUserServices _userServices;
        private readonly IProductServices _productServices;
        private readonly IOrderDetailServices _orderDetailServices;
        public OrderController(IOrderServices orderServices,
            IUserServices userServices,
            IProductServices productServices,
            IOrderDetailServices orderDetailServices)
        {
            _orderServices = orderServices;
            _userServices = userServices;
            _productServices = productServices;
            _orderDetailServices = orderDetailServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("/admin/json-order")]
        public async Task<IActionResult> Order()
        {
            var data = await _orderServices.GetAllAsync();
            return Ok(data);
        }
        [Route("/admin/json-order-detail/{Id}")]
        public async Task<IActionResult> OrderDetail(int Id)
        {
            var data = await _orderServices.GetByIdAsync(Id);
            return Ok(data);
        }
        [Route("/admin/json-order-delivery")]
        [HttpPost]
        public async Task<IActionResult> OrderDelivery(int Id)
        {
            try
            {
                var order = await _orderServices.GetOrderByIdAsync(Id);
                if (order == null)
                {
                    return StatusCode(404);
                }
                var _user = await _userServices.GetUserDetailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
                order.Status = true;
                await _orderServices.UpdateAsync(order);


                var _orderDetails = await _orderDetailServices.GetAllByOrderIdAsync(Id);
                foreach (var item in _orderDetails)
                {
                    var product = await _productServices.GetProductByIdAsync(item.ProductId);
                    product.Quantity -= item.Quanlity;
                    await _productServices.UpdateAsync(product);
                }

                return Json(new { success = true, message = "Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = ex.Message });
            }
        }
        [Route("/admin/json-order-deliverystatus")]
        [HttpPost]
        public async Task<IActionResult> OrderDeliveryStatus(int Id, int Status, int Code)
        {
            try
            {
                var order = await _orderServices.GetOrderByIdAsync(Id);
                if (order == null)
                {
                    return StatusCode(404);
                }
                var _user = await _userServices.GetUserDetailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
                order.IsActive = Convert.ToBoolean(Status);
                if(Status == 1 && Code == 0)
                {
                    order.OrderStatus = true;
                }
                await _orderServices.UpdateAsync(order);


                var _orderDetails = await _orderDetailServices.GetAllByOrderIdAsync(Id);
                foreach (var item in _orderDetails)
                {
                    var product = await _productServices.GetProductByIdAsync(item.ProductId);
                    if (Status == 0)
                    {
                        product.Quantity += item.Quanlity;
                    }
                    else
                    {
                        product.SoldItem += item.Quanlity;
                        if (Code == 0)
                        {
                            product.Quantity -= item.Quanlity;
                        }
                    }
                    await _productServices.UpdateAsync(product);
                }

                return Json(new { success = true, message = "Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = ex.Message });
            }
        }
    }
}
