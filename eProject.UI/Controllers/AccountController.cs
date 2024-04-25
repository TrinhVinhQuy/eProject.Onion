using eProject.Application.Abstracts;
using eProject.Application.DTOs.ViewModel;
using eProject.Domain.Entities;
using eProject.Insfrastructure.Services;
using eProject.UI.Ultility;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace eProject.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IResetPasswordServices _resetPasswordServices;
        private readonly IUserServices _userServices;
        public AccountController(IResetPasswordServices resetPasswordServices,
            IUserServices userServices)
        {
            _resetPasswordServices = resetPasswordServices;
            _userServices = userServices;
        }
        [Route("/quen-mat-khau")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("/quen-mat-khau")]
        public async Task<IActionResult> Index(string email)
        {
            var userCheck = await _userServices.GetUserDetailAsync(email);
            if (userCheck != null)
            {
                EmailService emailService = new EmailService();
                if (emailService.IsValidEmail(email))
                {
                    string host = HttpContext.Request.Host.Value;
                    string scheme = HttpContext.Request.Scheme;
                    var link = scheme + "://" + host;

                    Guid code = Guid.NewGuid();

                    var emailModel = new SendEmailModel
                    {
                        ToEmail = email,
                        Subject = "Đổi mật khẩu",
                        HtmlBody = "<h1> Vui lòng nhấn và liên kết sau để đổi mật khẩu</h1>" +
                        "<p>" +
                        "<a href='" + link + "/doi-mat-khau?code=" + code + "&email=" + email + "'>Đổi mật khẩu</a>" +
                        "</p>" +
                        "<span>Link chỉ tồn tại trong vòng 10 phút</span>"
                    };
                    emailService.EmailSend(emailModel);

                    var reset = new ResetPassword
                    {
                        Code = code,
                        CreatedOn = DateTime.Now,
                        UserId = userCheck.Id,
                        IsActive = false,
                    };
                    await _resetPasswordServices.InsertAsync(reset);

                    return Json(new { success = true, message = "Vui lòng kiểm tra email!" });
                }
                return Json(new { success = false, message = "Email không đúng định dạng!" });
            }
            return Json(new { success = false, message = "Email chưa được đăng kí!" });
        }
        [Route("/doi-mat-khau")]
        public async Task<IActionResult> Reset(Guid code, string email)
        {
            var userCheck = await _userServices.GetUserDetailAsync(email);
            var resetCheck = await _resetPasswordServices.GetByCodeAsync(code);
            if (userCheck != null && resetCheck != null)
            {
                ViewBag.Code = code;
                ViewBag.Email = email;
                return View();
            }
            return StatusCode(404);
        }
        [Route("/reset-mat-khau")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(Guid code, string email, string password)
        {
            var userCheck = await _userServices.GetUserDetailAsync(email);
            var resetCheck = await _resetPasswordServices.GetByCodeAsync(code);
            if (userCheck != null && resetCheck != null)
            {
                var timeCheck = DateTime.Now - resetCheck.CreatedOn;

                var passCheck = await _userServices.CheckDuplicatePass(Md5.ComputeMD5Hash(password));
                var passCheckReset = await _resetPasswordServices.CheckDuplicatePass(Md5.ComputeMD5Hash(password));
                if (passCheck || passCheckReset)
                {
                    return Json(new { success = false, message = "Mật khẩu này đã được đặt!" });
                }
                if (timeCheck.TotalSeconds <= 600)
                {
                    try
                    {
                        var reset = new ResetPassword
                        {
                            Id = resetCheck.Id,
                            Password = Md5.ComputeMD5Hash(password),
                            IsActive = true
                        };
                        await _resetPasswordServices.UpdateAsync(reset);
                        await _userServices.UpdatePassAsync(Md5.ComputeMD5Hash(password), userCheck.Id);
                        return Json(new { success = true, message = "Đã thay đổi mật khẩu thành công!" });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = "Fail: " + ex.Message });
                    }
                }
            }
            return StatusCode(404);
        }
    }
}
