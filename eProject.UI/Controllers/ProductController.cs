using eProject.Application.Abstracts;
using eProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eProject.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;
        public ProductController(IProductServices productServices, ICategoryServices categoryServices)
        {
            _productServices = productServices;
            _categoryServices = categoryServices;
        }
        public async Task<IActionResult> Index(string product)
        {
            var _products = await _productServices.GetByCategoryAsync();
            var _proDetail = _products.Where(x => x.MetaLink == product);
            if (_proDetail.Count() < 1)
            {
                return StatusCode(404);
            }
            var _cate = await _categoryServices.GetAllAsync();
            ViewBag.CategoryNameH2 = _cate.First(x=>x.Id == _proDetail.First().CategoryId).Name;
            ViewBag.CategoryLink = _cate.First(x=>x.Id == _proDetail.First().CategoryId).MetaLink;
            return View(_proDetail.First());
        }
    }
}
