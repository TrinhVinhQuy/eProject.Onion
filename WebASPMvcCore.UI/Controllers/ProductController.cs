using Microsoft.AspNetCore.Mvc;
using WebASPMvcCore.Application.Abstracts;
using WebASPMvcCore.Application.DTOs;

namespace WebASPMvcCore.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(string url)
        {
            var result = await _productService.GetProductDetailAsync(url);

            if (result == null)
            {
                return StatusCode(404);
            }

            var requestDatatable = new RequestDatatable();
            requestDatatable.PageSize = 12;
            requestDatatable.Page = 1;
            var reletadProduct = await _productService.GetBooksByPaginationAsync(requestDatatable);
            ViewBag.ReletadProduct = reletadProduct.Data.Take(4);

            ViewBag.CategoryName = reletadProduct.Data.First().CategoryName;
            ViewBag.CategoryLink = reletadProduct.Data.First().CategoryMetaLink;

            return View(result);
        }
    }
}
