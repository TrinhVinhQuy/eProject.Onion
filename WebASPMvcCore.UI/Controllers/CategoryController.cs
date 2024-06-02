using Microsoft.AspNetCore.Mvc;
using WebASPMvcCore.Application.Abstracts;
using WebASPMvcCore.Application.DTOs;
using WebASPMvcCore.Application.DTOs.Product;
using X.PagedList;

namespace WebASPMvcCore.UI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public CategoryController(IProductService productService,
            ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(int? page, string? search, string? url, string? sort)
        {
            ViewBag.CategoryProductCount = await _categoryService.GetAllCateProductCount();
            ViewBag.CategoryName = "Danh sách sản phẩm";

            var requestDatatable = new RequestDatatable();

            requestDatatable.PageSize = 12;
            requestDatatable.Page = page ?? 1;
            requestDatatable.Keyword = search;
            if(url != null)
            {
                requestDatatable.CategoryId = await _categoryService.GetCategoryIdByUrlAsync(url);
            }

            var result = await _productService.GetBooksByPaginationAsync(requestDatatable);

            ViewBag.ProductCount = result.RecordsTotal;

            ViewBag.TopProduct = await _productService.GetProductTopAsync();

            switch (sort)
            {
                case "asc":
                    result.Data = result.Data.OrderBy(x => x.Price);
                    break;
                case "desc":
                    result.Data = result.Data.OrderByDescending(x => x.Price);
                    break;
                case "name":
                    result.Data = result.Data.OrderBy(x => x.Name);
                    break;
                default:
                    break;
            }

            // Chuyển đổi danh sách thành IPagedList
            IPagedList<ProductDTO> pagedResult = new StaticPagedList<ProductDTO>(result.Data, requestDatatable.Page, requestDatatable.PageSize, result.RecordsTotal);

            return View(pagedResult);
        }
    }
}
