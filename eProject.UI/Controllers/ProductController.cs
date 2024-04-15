using Microsoft.AspNetCore.Mvc;

namespace eProject.UI.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index(int id ,string name)
        {
            ViewBag.Name = name + id;
            return View();
        }
    }
}
