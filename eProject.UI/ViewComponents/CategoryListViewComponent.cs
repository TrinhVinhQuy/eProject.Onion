using eProject.Application.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace eProject.UI.ViewComponents
{
    [ViewComponent(Name = "CategoryList")]
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly ICategoryServices _services;
        public CategoryListViewComponent(ICategoryServices services)
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
