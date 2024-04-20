using eProject.Application.Abstracts;
using eProject.Application.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace eProject.UI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;
        public CategoryController(IProductServices productServices, ICategoryServices categoryServices)
        {
            _productServices = productServices;
            _categoryServices = categoryServices;
        }
        //[Route("/tat-ca-san-pham")]
        public async Task<IActionResult> Index(string? url, int? page, string? query, string? search, int? priceMax, int? priceMin)
        {
            var _cate = await _categoryServices.GetAllAsync();
            ViewBag.Category = _cate;
            ViewBag.CategoryProductCount = await _categoryServices.GetCountProductCategoryAsync();
            var _product = await _productServices.GetByCategoryAsync();
            ViewBag.CountAllProduct = _product.Count();
            ViewBag.MaxPrice = _product.OrderByDescending(x => x.Price).First().Price;
            ViewBag.TopProduct = _product.OrderByDescending(x => x.SoldItem).Take(4);
            if (url == "all")
            {
                ViewBag.NameH1 = "Tất cả các sản phẩm";
            }
            if (url != "all" && url != null)
            {

                var _cateId = _cate.Where(x => x.MetaLink == url);
                if (_cateId.Count() < 1)
                {
                    return StatusCode(404);
                }
                ViewBag.NameH1 = _cateId.First().Name;
                _product = _product.Where(x => x.CategoryId == _cateId.First().Id);
            }
            switch (query)
            {
                case "asc":
                    _product = _product.OrderBy(x => x.Price);
                    break;
                case "desc":
                    _product = _product.OrderByDescending(x => x.Price);
                    break;
                case "name":
                    _product = _product.OrderBy(x => x.Name);
                    break;
                default:
                    break;
            }
            int pageSize = 1;
            int pageNumber = page ?? 1;
            if (!string.IsNullOrEmpty(search))
            {
                _product = _product.Where(x => x.Name.ToLower().Contains(search.ToLower()) || x.MetaKeywords.ToLower().Contains(search.ToLower()));
            }
            if (priceMax > 0)
            {
                _product = _product.Where(x => x.Price <= priceMax && x.Price >= priceMin);
            }
            _product = _product.AsQueryable().ToPagedList(pageNumber, pageSize);
            return View(_product);
        }
        public async Task<IActionResult> IndexProduct()
        {
            var _product = await _productServices.GetByCategoryAsync();
            return Json(_product);
        }
    }
}
