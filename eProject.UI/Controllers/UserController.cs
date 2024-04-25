using AutoMapper;
using eProject.Application.Abstracts;
using eProject.Application.DTOs.User;
using eProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eProject.UI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        public async Task<IActionResult> Index()
        {
            var _user = await _userServices.GetUserDetailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            return View(_user);
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserDetailDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Email = User.FindFirst(ClaimTypes.Email).Value;
                    model.IsActive = true;
                    await _userServices.UpdateAsync(model);
                    return Json(new { success = true, message = "Cập nhật thành công!" });
                }
                return StatusCode(404);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Fail: " + ex });
            }
        }
    }
}
