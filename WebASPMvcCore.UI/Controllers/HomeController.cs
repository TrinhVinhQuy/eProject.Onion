using Microsoft.AspNetCore.Mvc;
using WebASPMvcCore.Application.Abstracts;
using WebASPMvcCore.Application.DTOs;
namespace WebASPMvcCore.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        public HomeController(ICategoryService categoryService, 
            IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Category = await _categoryService.GetAllAsync();

            var requestDatatable = new RequestDatatable();
            requestDatatable.PageSize = 12;
            requestDatatable.Page = 1;
            var productDiscount = await _productService.GetBooksByPaginationAsync(requestDatatable);
            ViewBag.ProductDiscount = productDiscount.Data.Where(x=>x.Discount>0).ToList();

            return View();
        }

        public IActionResult Introduce()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
