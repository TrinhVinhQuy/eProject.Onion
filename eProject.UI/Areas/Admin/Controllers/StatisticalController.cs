using eProject.Application.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace eProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StatisticalController : Controller
    {
        private readonly IOrderServices _orderServices;
        public StatisticalController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("/admin/statistical-day")]
        public async Task<IActionResult> StatisticalDay(DateTime dateTime)
        {
            var data = await _orderServices.GetAllAsync();
            data = data.Where(x => x.IsActive == true && x.CreateOn.Day == dateTime.Day);
            return Ok(data);
        }
        [HttpPost]
        [Route("/admin/statistical-month")]
        public async Task<IActionResult> StatisticalMonth(int month)
        {
            var data = await _orderServices.GetAllAsync();
            data = data.Where(x => x.IsActive == true && x.CreateOn.Month == month && x.CreateOn.Year == DateTime.Now.Year);
            return Ok(data);
        }
        public IActionResult Chart()
        {
            return View();
        }
        [Route("/admin/statistical-chart")]
        public async Task<IActionResult> StatisticalChart()
        {
            var order = await _orderServices.GetAllAsync();
            order = order.Where(x => x.IsActive == true && x.CreateOn.Year == DateTime.Now.Year);
            decimal[] orderTotalYears = new decimal[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (var item in order)
            {
                var orderDetail = await _orderServices.GetByIdAsync(item.Id);
                orderTotalYears[item.CreateOn.Month - 1] = orderDetail.Products.Sum(x => x.Price);
            }
            return Ok(orderTotalYears);
        }
    }
}
