using eProject.Application.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private IOrderServices _orderServices;
        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
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
    }
}
