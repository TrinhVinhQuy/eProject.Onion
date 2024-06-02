using Microsoft.AspNetCore.Mvc;
using WebASPMvcCore.Application.Abstracts;

namespace WebASPMvcCore.UI.ViewComponents
{
    [ViewComponent(Name = "CategoryList")]
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly ICategoryService _services;
        public CategoryListViewComponent(ICategoryService services)
        {
            _services = services;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cate = await _services.GetAllAsync();
            return View(cate);
        }
    }
}
