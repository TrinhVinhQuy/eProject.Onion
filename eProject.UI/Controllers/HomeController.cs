using AutoMapper;
using eProject.Application.Abstracts;
using eProject.Application.DTOs.Category;
using eProject.Application.DTOs.Role;
using eProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eProject.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;
        public HomeController(ICategoryServices categpryServices, IMapper mapper)
        {
            _categoryServices = categpryServices;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            string host = HttpContext.Request.Host.Value;
            string scheme = HttpContext.Request.Scheme;
            ViewData["Host"] = scheme + "://" + host;

            var _cate = await _categoryServices.GetAllAsync();
            ViewBag.Category = _cate;

            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
