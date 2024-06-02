using AutoMapper;
using WebASPMvcCore.Application.DTOs.Category;
using WebASPMvcCore.Application.DTOs.Order;
using WebASPMvcCore.Application.DTOs.Product;
using WebASPMvcCore.Domain.Entities;

namespace WebASPMvcCore.Application.Configuration
{
    public class AutomapConfig : Profile
    {
        public AutomapConfig()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();

            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductViewDTO>().ReverseMap();
            CreateMap<Product, ProductCartDTO>().ReverseMap();
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();
            CreateMap<ProductDetailDTO, Product>().ReverseMap();

            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Order, OrderShowDTO>().ReverseMap();
        }
    }
}
