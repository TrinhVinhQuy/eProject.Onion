using Microsoft.AspNetCore.Mvc;

namespace eProject.UI.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
