using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebASPMvcCore.Application.Abstracts;
using WebASPMvcCore.Application.Services;
using WebASPMvcCore.Domain.Entities;
using WebASPMvcCore.Domain.Enums;

namespace WebASPMvcCore.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController(IOrderService orderService,
            UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("/admin/json-order")]
        public async Task<IActionResult> Order()
        {
            var data = await _orderService.GetAllOrderAsync();
            return Ok(data);
        }
        [Route("/admin/json-order-detail/{Id}")]
        public async Task<IActionResult> OrderDetail(Guid Id)
        {
            var data = await _orderService.GetOrderDetailByIdAsync(Id);
            return Ok(data);
        }
        [Route("/admin/json-order-status-processing")]
        [HttpPost]
        public async Task<IActionResult> OrderStatusProcessing(Guid Id, StatusProcessing StatusProcessing)
        {

            if (Id == Guid.Empty) return BadRequest();

            var result = await _orderService.UpdateOrderStatusProcessingAsync(Id, StatusProcessing);
            if (result)
            {
                return Json(new { success = true, message = "Cập nhật thành công!" });
            }
            else
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra!" });
            }
        }
    }
}
