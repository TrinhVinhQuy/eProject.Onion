using eProject.Application.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace eProject.UI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IResetPasswordServices resetPasswordServices;
        public AccountController(IResetPasswordServices resetPasswordServices)
        {
            this.resetPasswordServices = resetPasswordServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string email)
        {
            return View();
        }
    }
}
