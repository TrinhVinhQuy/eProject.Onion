﻿using AutoMapper;
using eProject.Application.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace eProject.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryServices _categoryServices;
        private readonly IProductServices _productServices;
        private readonly IMapper _mapper;
        public HomeController(ICategoryServices categpryServices, IMapper mapper, IProductServices productServices)
        {
            _categoryServices = categpryServices;
            _mapper = mapper;
            _productServices = productServices;
        }
        [Route("")]
        [Route("home")]
        [Route("trang-chu")]
        public async Task<IActionResult> Index()
        {
            string host = HttpContext.Request.Host.Value;
            string scheme = HttpContext.Request.Scheme;
            ViewData["Host"] = scheme + "://" + host;

            var _cate = await _categoryServices.GetAllAsync();
            ViewBag.Category = _cate;

            var _productDiscount = await _productServices.GetAllAsync();
            ViewBag.ProductDiscount = _productDiscount.Where(x => x.Discount > 0);

            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
