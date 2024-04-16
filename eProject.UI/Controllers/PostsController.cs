using Microsoft.AspNetCore.Mvc;

namespace eProject.UI.Controllers
{
    public class PostsController : Controller
    {
        public IActionResult Index(string posts)
        {
            ViewBag.Url = posts;
            return View();
        }
    }
}
