using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using WebASPMvcCore.Application.Abstracts;
using WebASPMvcCore.Application.DTOs.Order;
using WebASPMvcCore.Domain.Entities;
using WebASPMvcCore.Insfrastructure.Services;
using WebASPMvcCore.Insfrastructure.Services.VnPay;
using WebASPMvcCore.UI.Models;
using WebASPMvcCore.UI.Ultility;

namespace WebASPMvcCore.UI.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        public CheckOutController(UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext,
            IConfiguration configuration,
            IOrderService orderService,
            IProductService productService)
        {
            _userManager = userManager;
            _hubContext = hubContext;
            _configuration = configuration;
            _orderService = orderService;
            _productService = productService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Index(string Province, string District, string Town, string Address, double Total)
        {
            if (Province == "" || District == "" || Town == "" || Address == "")
            {
                return StatusCode(404);
            }
            else
            {
                HttpContext.Session.SetString("Province", Province);
                HttpContext.Session.SetString("District", District);
                HttpContext.Session.SetString("Town", Town);
                HttpContext.Session.SetString("Address", Address);
            }
            string vnp_Returnurl = _configuration.GetSection("Payments:VnPay")["vnp_Returnurl"];
            string vnp_Url = _configuration.GetSection("Payments:VnPay")["vnp_Url"];
            string vnp_TmnCode = _configuration.GetSection("Payments:VnPay")["vnp_TmnCode"];
            string vnp_HashSecret = _configuration.GetSection("Payments:VnPay")["vnp_HashSecret"];

            var CartModels = HttpContext.Session.Get<List<CartModel>>("Cart");

            if (CartModels == null || CartModels.Count() < 1)
            {
                return StatusCode(404);
            }
            
            OrderInfo order = new OrderInfo
            {
                OrderId = DateTime.Now.Ticks,
                Amount = Total,
                Status = "0",
                CreatedDate = DateTime.Now
            };
            HttpContext.Session.SetString("vnp_TxnRef", order.OrderId.ToString());
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(HttpContext));
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString());
            vnpay.AddRequestData("vnp_Locale", "vn");

            string paymentUrl = await Task.Run(() => vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret)); // Thực hiện tạo URL thanh toán bất đồng bộ
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return Redirect(paymentUrl);
        }
        [Authorize]
        public async Task<IActionResult> ConfirmPay([FromQuery] ConfirmVnPayModel model)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("vnp_TxnRef") != model.vnp_TxnRef)
                {
                    return StatusCode(404);
                }
                if (model.vnp_ResponseCode == "00" && model.vnp_TransactionStatus == "00")
                {
                    var CartModels = HttpContext.Session.Get<List<CartModel>>("Cart");
                    
                    var _user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Name).Value);

                    var items = new List<OrderDetailDTO>();
                    var orderId = Guid.NewGuid();

                    var _products = await _productService.GetAllProductCartsAsync();
                    foreach (var cart in CartModels)
                    {
                        var item = _products.First(x => x.Id == cart.Id);
                        items.Add(new OrderDetailDTO
                        {
                            Id = Guid.NewGuid(),
                            ProductId = cart.Id,
                            OrderId = orderId,
                            UnitPrice = item.Price - item.Price * item.Discount / 100,
                            Quanlity = cart.Quantity,
                        });
                    }
                    var _order = new OrderDTO
                    {
                        Id = orderId,
                        UserId = _user.Id,
                        CreateOn = DateTime.Now,
                        Province = HttpContext.Session.GetString("Province"),
                        District = HttpContext.Session.GetString("District"),
                        Town = HttpContext.Session.GetString("Town"),
                        Address = HttpContext.Session.GetString("Address"),
                        TotalAmount = Convert.ToDouble(model.vnp_Amount),
                        PaymentMethod = (Domain.Enums.PaymentMethod)2,
                        StatusProcessing = (Domain.Enums.StatusProcessing)2,
                        Items = items,
                    };
                    
                    await _orderService.SaveAsync(_order);
                    
                    ViewBag.ResponseCode = "00";
                    HttpContext.Session.Remove("Province");
                    HttpContext.Session.Remove("District");
                    HttpContext.Session.Remove("Town");
                    HttpContext.Session.Remove("Address");
                    HttpContext.Session.Remove("Cart");
                    HttpContext.Session.Remove("vnp_TxnRef");
                    await _hubContext.Clients.All.SendAsync("OrderHub");
                    return RedirectToAction("Index", "HistoryOrder");
                }
                else
                {
                    ViewBag.ResponseCode = model.vnp_ResponseCode == "07" ? "Trừ tiền thành công. Giao dịch bị nghi ngờ (liên quan tới lừa đảo, giao dịch bất thường)." : null;
                    ViewBag.ResponseCode = model.vnp_ResponseCode == "09" ? "Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng chưa đăng ký dịch vụ InternetBanking tại ngân hàng." : null;
                    ViewBag.ResponseCode = model.vnp_ResponseCode == "10" ? "Giao dịch không thành công do: Khách hàng xác thực thông tin thẻ/tài khoản không đúng quá 3 lần" : null;
                    ViewBag.ResponseCode = model.vnp_ResponseCode == "11" ? "Giao dịch không thành công do: Đã hết hạn chờ thanh toán. Xin quý khách vui lòng thực hiện lại giao dịch." : null;
                    ViewBag.ResponseCode = model.vnp_ResponseCode == "12" ? "Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng bị khóa." : null;
                    ViewBag.ResponseCode = model.vnp_ResponseCode == "13" ? "Giao dịch không thành công do Quý khách nhập sai mật khẩu xác thực giao dịch (OTP). Xin quý khách vui lòng thực hiện lại giao dịch." : null;
                    ViewBag.ResponseCode = model.vnp_ResponseCode == "24" ? "Giao dịch không thành công do: Khách hàng hủy giao dịch" : null;
                    ViewBag.ResponseCode = model.vnp_ResponseCode == "51" ? "Giao dịch không thành công do: Tài khoản của quý khách không đủ số dư để thực hiện giao dịch." : null;
                    ViewBag.ResponseCode = model.vnp_ResponseCode == "65" ? "Giao dịch không thành công do: Tài khoản của Quý khách đã vượt quá hạn mức giao dịch trong ngày." : null;
                    ViewBag.ResponseCode = model.vnp_ResponseCode == "75" ? "Ngân hàng thanh toán đang bảo trì." : null;
                    ViewBag.ResponseCode = model.vnp_ResponseCode == "79" ? "Giao dịch không thành công do: KH nhập sai mật khẩu thanh toán quá số lần quy định. Xin quý khách vui lòng thực hiện lại giao dịch" : null;
                    ViewBag.ResponseCode = model.vnp_ResponseCode == "99" ? "Có lỗi vui lòng liên hệ với Admin" : null;
                    return View();
                }
            }
            return StatusCode(404);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PayCod(string Province, string District, string Town, string Address, double Total)
        {
            if (Province == "" || District == "" || Town == "" || Address == "")
            {
                return StatusCode(404);
            }
            var CartModels = HttpContext.Session.Get<List<CartModel>>("Cart") ?? new List<CartModel>();
            if (CartModels.Count() > 0)
            {
                var _user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Name).Value);

                var items = new List<OrderDetailDTO>();
                var orderId = Guid.NewGuid();

                var _products = await _productService.GetAllProductCartsAsync();
                foreach (var cart in CartModels)
                {
                    var item = _products.First(x => x.Id == cart.Id);
                    items.Add(new OrderDetailDTO
                    {
                        Id = Guid.NewGuid(),
                        ProductId = cart.Id,
                        OrderId = orderId,
                        UnitPrice = item.Price - item.Price * item.Discount / 100,
                        Quanlity = cart.Quantity,
                    });
                }
                var _order = new OrderDTO
                {
                    Id = orderId,
                    UserId = _user.Id,
                    CreateOn = DateTime.Now,
                    Province = Province,
                    District = District,
                    Town = Town,
                    Address = Address,
                    TotalAmount = Total,
                    PaymentMethod = (Domain.Enums.PaymentMethod)1,
                    StatusProcessing = (Domain.Enums.StatusProcessing)2,
                    Items = items,
                };

                bool result = await _orderService.SaveAsync(_order);

                if (result == true)
                {
                    HttpContext.Session.Remove("Cart");
                    await _hubContext.Clients.All.SendAsync("OrderHub");
                    return RedirectToAction("Index", "HistoryOrder");
                }
            }
            return StatusCode(404);
        }
    }
}
