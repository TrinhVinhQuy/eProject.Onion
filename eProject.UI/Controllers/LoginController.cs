using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using eProject.UI.Models;
using eProject.Application.Abstracts;
using eProject.UI.Ultility;
using eProject.Domain.Entities;
using Microsoft.AspNetCore.Authentication.Google;
using eProject.Insfrastructure.Services;
using System.Text.RegularExpressions;

namespace eProject.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserServices _userServices;
        public LoginController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        public IActionResult Index(string? ReturnUrl)
        {
            if (ReturnUrl != null)
            {
                ViewBag.ReturnUrl = "Vui lòng đăng nhập với quyền quản trị!";
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginModel model)
        {
            // Quy@0104
            if (ModelState.IsValid)
            {
                var pass = Md5.ComputeMD5Hash(model.Password);
                var _users = await _userServices.GetAllUserLoginAsync();
                var user = _users.FirstOrDefault(x => x.UserName.ToLower().Contains(model.Username.ToLower()) && x.Password == Md5.ComputeMD5Hash(model.Password));
                if (user == null)
                {
                    ViewData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                    return View(model);
                }
                if (user.IsActive == false)
                {
                    ViewData["ErrorMessage"] = "Tài khoản của bạn đã bị khoá vui lòng liên hệ Admin để biết thêm!";
                    return View(model);
                }
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Role, user.RoleName)
                    };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse", "Login")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (result.Succeeded)
            {
                var emailClaim = result.Principal.FindFirst(ClaimTypes.Email)?.Value;

                var _user = await _userServices.GetUserDetailAsync(emailClaim);
                if (_user == null)
                {
                    var user = new User
                    {
                        Email = emailClaim,
                        Name = result.Principal.FindFirst(ClaimTypes.Name)?.Value,
                        RoleId = 2,
                    };
                    await _userServices.InsertAsync(user);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Route("/register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.CfPassword != model.Password)
                {
                    return Json(new { Success = false, message = "Mật khẩu không khớp!" });
                }
                if (IsStrongPassword(model.Password))
                {
                    return Json(new { Success = false, message = "Mật khẩu chứa ít nhất 6 kí tự, chữ hoa, chữ thường,số ,kí tự đặc biệt!" });
                }
                var _userCheck = await _userServices.GetUserDetailAsync(model.Email);
                if (_userCheck != null)
                {
                    return Json(new { Success = false, message = "Email đã tồn tại" });
                }
                var _users = await _userServices.GetAllUserLoginAsync();
                if (_users.Where(x => x.UserName.ToLower() == model.Username.ToLower()).Count() > 0)
                {
                    return Json(new { Success = false, message = "Username đã tồn tại" });
                }
                OtpService otp = new OtpService();
                bool success = VerifyOtp(model.Otp);
                if (success)
                {
                    try
                    {
                        var user = new User
                        {
                            Email = model.Email,
                            UserName = model.Username,
                            Name = model.Name,
                            Password = Md5.ComputeMD5Hash(model.Password),
                            RoleId = 2,
                            IsActive = true,
                        };
                        await _userServices.InsertAsync(user);
                        return Json(new { Success = true, message = "Đăng kí thành công!" });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { Success = false, message = "Fail :" + ex.Message });
                    }
                }
                return Json(new { Success = false, message = "Otp không khớp!" });
            }
            return Json(new { Success = false, message = "fail!" });
        }
        [Route("/send-otp")]
        [HttpPost]
        public async Task<IActionResult> RegisterOTP(string email)
        {
            EmailService emailService = new EmailService();
            if (emailService.IsValidEmail(email))
            {
                var _userCheck = await _userServices.GetUserDetailAsync(email);
                if (_userCheck != null)
                {
                    return Json(new { Success = false, message = "Email đã tồn tại" });
                }
                OtpService otp = new OtpService();
                var _otp = otp.GenerateOtp(email);
                var otpModel = new OtpModel
                {
                    Otp = _otp,
                    StartTime = DateTime.Now,
                };

                var _otpSession = GetSessionOTP();
                _otpSession.Add(otpModel);
                SetSessionOTP(_otpSession);

                return Json(new { Success = true, message = "Mã OTP sẽ tồn tại trong 360 giây!" });
            }
            return Json(false);
        }
        public bool VerifyOtp(string otp)
        {
            // Lấy danh sách các mã OTP từ session, nếu không có thì tạo mới
            var otpSession = GetSessionOTP();

            // Kiểm tra xem có mã OTP trong danh sách không
            if (otpSession.Any())
            {
                // Lấy thời gian hiện tại
                var currentTime = DateTime.Now;

                // Lấy thời gian khi OTP đầu tiên được tạo ra
                var startTime = otpSession.First().StartTime;

                // Tính thời gian kể từ khi OTP được tạo ra
                var timeElapsed = currentTime - startTime;

                // Kiểm tra xem thời gian đã trôi qua có lớn hơn 360 giây (6 phút) hay không
                if (timeElapsed.TotalSeconds <= 360)
                {
                    // Kiểm tra xem OTP nhập vào có trùng khớp với OTP đầu tiên trong danh sách không
                    if (otpSession.First().Otp == otp)
                    {
                        otpSession.RemoveAll(x => x.Otp == otp);
                        return true; // Nếu trùng khớp và thời gian còn hợp lệ, trả về true
                    }
                }
                else
                {
                    // Nếu thời gian đã trôi qua hơn 360 giây, loại bỏ mã OTP từ danh sách
                    otpSession.RemoveAll(x => x.Otp == otp);
                }
            }
            return false; // Trả về false nếu không tìm thấy mã OTP hoặc mã OTP đã hết hạn
        }

        
        private bool IsStrongPassword(string password)
        {
            // Kiểm tra độ dài mật khẩu (ít nhất 6 ký tự)
            if (password.Length > 5)
                return false;

            // Kiểm tra xem mật khẩu có chứa ít nhất một chữ cái viết hoa
            if (!password.Any(char.IsUpper))
                return false;

            // Kiểm tra xem mật khẩu có chứa ít nhất một chữ cái viết thường
            if (!password.Any(char.IsLower))
                return false;

            // Kiểm tra xem mật khẩu có chứa ít nhất một số
            if (!password.Any(char.IsDigit))
                return false;

            // Kiểm tra xem mật khẩu có chứa ít nhất một ký tự đặc biệt
            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
                return false;

            return true;
        }
        private List<OtpModel>? GetSessionOTP()
        {
            return HttpContext.Session.Get<List<OtpModel>>("OtpEmail") ?? new List<OtpModel>(); ;
        }

        private void SetSessionOTP(List<OtpModel> carts)
        {
            HttpContext.Session.Set("OtpEmail", carts);
        }
    }
}
