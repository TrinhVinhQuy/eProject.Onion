using eProject.UI.Models;
using eProject.UI.Ultility;
using Microsoft.AspNetCore.Mvc;

namespace eProject.UI.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Route("/json/cart")]
        public IActionResult JsonCart()
        {
            var carts = GetSessionCart();
            return Json(carts);
        }
        [HttpPost]
        public IActionResult AddCart(CartModel cart)
        {
            try
            {
                var carts = GetSessionCart() ?? new List<CartModel>();
                if (!carts.Any())
                {
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
                        if (cartExist.Quantity < 1)
                        {
                            carts.RemoveAll(x => x.Id == cart.Id);
                        }
                    }
                }
                SetSessionCart(carts);
                return Json(carts.Count);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public IActionResult DeleteCart(int Id)
        {
            try
            {
                var carts = GetSessionCart();
                if (carts is not null)
                {
                    carts.RemoveAll(x => x.Id == Id);
                    SetSessionCart(carts);
                }
                return Json(true);
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
