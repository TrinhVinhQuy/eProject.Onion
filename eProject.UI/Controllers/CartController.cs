using AutoMapper;
using eProject.Application.Abstracts;
using eProject.Application.DTOs.Product;
using eProject.UI.Models;
using eProject.UI.Ultility;
using Microsoft.AspNetCore.Mvc;

namespace eProject.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly IMapper _mapper;
        public CartController(IProductServices productServices, IMapper mapper)
        {
            _productServices = productServices;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("/json/cart")]
        public async Task<IActionResult> JsonCart()
        {
            var carts = GetSessionCart();
            if (carts == null || carts.Count() < 1)
            {
                return Json(new { success = false, message = "Giỏ hàng đang trống!" });
            }
            var _products = await _productServices.GetAllAsync();
            var cartProductIds = carts.Select(cart => cart.Id);
            var results = _mapper.Map<IEnumerable<ProductCart>>(_products.Where(x => cartProductIds.Contains(x.Id)));
            foreach (var item in results)
            {
                item.Quantity = carts.First(x => x.Id == item.Id).Quantity;
                item.Price = item.Price - item.Price * item.Discount / 100;
                
            }
            var Total = results.Sum(x => x.Price * x.Quantity);
            return Json(new { success = true, results, Total });
        }
        [HttpPost]
        public async Task<IActionResult> AddCart(CartModel cart)
        {
            try
            {
                var _product = await _productServices.GetByIdAsync(cart.Id);
                var carts = GetSessionCart() ?? new List<CartModel>();
                if (!carts.Any())
                {
                    if (cart.Quantity < 0)
                    {
                        return Json(new { success = false, message = "Vui lòng không thêm số âm!" });
                    }
                    carts.Add(cart);
                }
                else
                {
                    var cartExist = carts.SingleOrDefault(x => x.Id == cart.Id);
                    if (cartExist is null)
                    {
                        carts.Add(cart);
                    }
                    else
                    {
                        cartExist.Quantity += cart.Quantity;
                        if (cartExist.Quantity > _product.Quantity)
                        {
                            return Json(new { success = false, message = "Sản phẩm đã đạt giới hạn trong kho!" });
                        }
                        if (cartExist.Quantity < 1)
                        {
                            carts.RemoveAll(x => x.Id == cart.Id);
                            SetSessionCart(carts);
                            return Json(new { success = false, message = "Sản phẩm trong giỏ hàng đã được xoá!" });
                        }
                    }
                }
                SetSessionCart(carts);
                return Json(new { success = true, message = "Đã thêm sản phẩm vào giỏ hàng" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Fail :" + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteCart(int Id)
        {
            try
            {
                var carts = GetSessionCart();
                if (carts is not null && carts.Any(x => x.Id == Id))
                {
                    carts.RemoveAll(x => x.Id == Id);
                    SetSessionCart(carts);
                    return Json(new { success = true, message = "Xoá sản phẩm giỏ hàng thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng" });
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        private List<CartModel>? GetSessionCart()
        {
            return HttpContext.Session.Get<List<CartModel>>("Cart");
        }

        private void SetSessionCart(List<CartModel> carts)
        {
            HttpContext.Session.Set("Cart", carts);
        }
    }
}
