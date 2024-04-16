using eProject.Application.Abstracts;
using eProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eProject.UI.Controllers
{
    public class ProductController : Controller
    {
        public async Task<IActionResult> Index(string product)
        {
            ViewBag.Url = product;
            return View();
        }
    }
}
