using eProject.Application.Abstracts;
using eProject.Domain.Entities;
using eProject.Insfrastructure.Services;
using eProject.Insfrastructure.Services.VnPay;
using eProject.UI.Models;
using eProject.UI.Ultility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace eProject.UI.Controllers
{
    [Authorize]
    public class VnPayController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IOrderDetailServices _orderDetailServices;
        private readonly IOrderServices _orderServices;
        private readonly IProductServices _productServices;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IConfiguration _configuration;
        public VnPayController(IUserServices userServices,
            IOrderDetailServices orderDetailServices,
            IOrderServices orderServices,
            IProductServices productServices,
            IHubContext<NotificationHub> hubContext,
            IConfiguration configuration)
        {
            _userServices = userServices;
            _orderDetailServices = orderDetailServices;
            _orderServices = orderServices;
            _productServices = productServices;
            _hubContext = hubContext;
            _configuration = configuration;
        }
        [Route("/VnPay")]
        [Route("/VnPay/Index")]
        [HttpPost]
        public async Task<IActionResult> Index(string Province, string District, string Town, string Address)
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

            var _products = await _productServices.GetAllAsync();

            double totalPrice = 0;
            foreach (var item in CartModels)
            {
                var pro = _products.First(x => x.Id == item.Id);
                var price = pro.Price;
                var discount = pro.Discount;
                totalPrice += Convert.ToDouble(price * (100 - discount) * item.Quantity / 100);
            }
            OrderInfo order = new OrderInfo
            {
                OrderId = DateTime.Now.Ticks,
                Amount = totalPrice,
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
                    var _user = await _userServices.GetUserDetailAsync(User.FindFirst(ClaimTypes.Email)?.Value);

                    var _order = new Order
                    {
                        UserId = _user.Id,
                        CreateOn = DateTime.Now,
                        Status = false,
                        OrderStatus = true,
                        InvoiceNumber = model.vnp_TxnRef,
                        TradingCode = model.vnp_TransactionNo,
                        Province = HttpContext.Session.GetString("Province"),
                        District = HttpContext.Session.GetString("District"),
                        Town = HttpContext.Session.GetString("Town"),
                        Address = HttpContext.Session.GetString("Address"),
                    };
                    var _orederId = await _orderServices.InsertAsync(_order);


                    foreach (var item in CartModels)
                    {
                        var _product = await _productServices.GetByIdAsync(item.Id);
                        var _orderDetail = new OrderDetail
                        {
                            OrderId = _orederId.Id,
                            ProductId = item.Id,
                            Price = _product.Price * (100 - _product.Discount) / 100,
                            Quanlity = item.Quantity,
                        };
                        await _orderDetailServices.InsertAsync(_orderDetail);

                        var product = await _productServices.GetProductByIdAsync(item.Id);
                        product.Quantity -= item.Quantity;
                        await _productServices.UpdateAsync(product);
                    }

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
        public async Task<IActionResult> PayCod(string Province, string District, string Town, string Address)
        {
            if (Province == "" || District == "" || Town == "" || Address == "")
            {
                return StatusCode(404);
            }
            var CartModels = HttpContext.Session.Get<List<CartModel>>("Cart") ?? new List<CartModel>();
            if (CartModels.Count() > 0)
            {
                var _user = await _userServices.GetUserDetailAsync(User.FindFirst(ClaimTypes.Email)?.Value);

                var _order = new Order
                {
                    UserId = _user.Id,
                    CreateOn = DateTime.Now,
                    Status = false,
                    OrderStatus = false,
                    Province = Province,
                    District = District,
                    Town = Town,
                    Address = Address,
                    InvoiceNumber = "0",
                    TradingCode = "0",
                };
                var _orederId = await _orderServices.InsertAsync(_order);


                foreach (var item in CartModels)
                {
                    var _product = await _productServices.GetByIdAsync(item.Id);
                    var _orderDetail = new OrderDetail
                    {
                        OrderId = _orederId.Id,
                        ProductId = item.Id,
                        Price = _product.Price * (100 - _product.Discount) / 100,
                        Quanlity = item.Quantity,
                        IsActive = true,
                    };
                    await _orderDetailServices.InsertAsync(_orderDetail);
                }
                HttpContext.Session.Remove("Cart");
                await _hubContext.Clients.All.SendAsync("OrderHub");
                return RedirectToAction("Index", "HistoryOrder");
            }
            return StatusCode(404);
        }
    }
}
