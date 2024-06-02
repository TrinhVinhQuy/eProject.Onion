using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebASPMvcCore.Application.Abstracts;
using WebASPMvcCore.Application.DTOs.Product;
using WebASPMvcCore.Domain.Entities;
using WebASPMvcCore.Insfrastructure.Abstracts;
using WebASPMvcCore.Insfrastructure.Services;

namespace WebASPMvcCore.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IHelperService _helper;
        private readonly IMapper _mapper;
        public ProductController(IProductService productService,
            IHubContext<NotificationHub> hubContext,
            IHelperService helper,
            ICategoryService categoryService,
            IMapper mapper)
        {
            _productService = productService;
            _hubContext = hubContext;
            _helper = helper;
            _categoryService = categoryService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("/admin/product-get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetAllProductAsync();
            return Ok(result);
        }

        public async Task<IActionResult> Create()
        {
            var cate = await _categoryService.GetAllAsync();
            ViewBag.Category = cate;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewDTO model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product();
                _mapper.Map(model, product);

                product.MetaTitle = model.Name;
                product.MetaLink = _helper.ConvertToSlug(model.Name);
                product.IsActive = true;

                var result = await _productService.CreateAsync(product, model.Images);
                if (result)
                {
                    await _hubContext.Clients.All.SendAsync("NewProductHub");
                    return Json(new { success = true, message = "Thêm sản phẩm thành công!" });
                }
                return Json(new { success = true, message = "Đã có lỗi xảy ra!" });
            }
            return StatusCode(404);
        }

        public async Task<IActionResult> Detail(Guid Id)
        {
            var cate = await _categoryService.GetAllAsync();
            ViewBag.Category = cate;
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Detail(ProductUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                model.MetaLink = _helper.ConvertToSlug(model.Name);
                var result = await _productService.GetProductUpdateAsync(model);
                return Json(result);
            }
            return Json(false);
        }


        [Route("/admin/product-get-id")]
        public async Task<IActionResult> DetailByUrl(Guid Id)
        {
            var productUrl = await _productService.GetProductDetailByIdAsync(Id);
            var result = await _productService.GetProductDetailAsync(productUrl.MetaLink);
            if (result == null) return Json(new { success = false });
            return Json(new { success = true, result });
        }

        [Route("/admin/product-isactive-id")]
        [HttpPost]
        public async Task<IActionResult> IsActive(Guid Id, bool IsActive)
        {
            var result = await _productService.IsActiveAsync(Id, IsActive);
            return Json(result);
        }
    }
}
