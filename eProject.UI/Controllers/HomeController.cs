using eProject.Application.Abstracts;
using eProject.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace eProject.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRoleServices _roleServices;
        private readonly IWebHostEnvironment _env;
        public HomeController(ILogger<HomeController> logger, IRoleServices roleServices, IWebHostEnvironment env)
        {
            _logger = logger;
            _roleServices = roleServices;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            string host = HttpContext.Request.Host.Value;
            string scheme = HttpContext.Request.Scheme;
            ViewData["Host"] = scheme + "://" + host;

            ViewBag.Role = await _roleServices.GetAll();
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
