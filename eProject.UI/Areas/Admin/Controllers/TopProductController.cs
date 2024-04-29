using eProject.Application.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TopProductController : Controller
    {
        private readonly IProductServices _productsService;
        public TopProductController(IProductServices productsService)
        {
            _productsService = productsService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("/admin/top-product")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productsService.GetAllAsync();
            result = result.OrderByDescending(x => x.SoldItem);
            return Ok(result);
        }
    }
}
