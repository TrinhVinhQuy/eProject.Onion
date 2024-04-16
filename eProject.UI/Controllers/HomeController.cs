using eProject.Application.Abstracts;
using eProject.Application.DTOs.Role;
using eProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eProject.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBaseServices<Role> _roleServices;
        private readonly IWebHostEnvironment _env;
        public HomeController(IBaseServices<Role> roleServices, IWebHostEnvironment env)
        {
            _roleServices = roleServices;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            string host = HttpContext.Request.Host.Value;
            string scheme = HttpContext.Request.Scheme;
            ViewData["Host"] = scheme + "://" + host;

            ViewBag.Role = await _roleServices.GetAllAsync();
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
