using AutoMapper;
using eProject.Application.DTOs.Category;
using eProject.Application.DTOs.Order;
using eProject.Application.DTOs.OrderDetail;
using eProject.Application.DTOs.Product;
using eProject.Application.DTOs.Role;
using eProject.Application.DTOs.User;
using eProject.Domain.Entities;

namespace eProject.Application.Configuration
{
    public class AutomapConfig : Profile
    {
        public AutomapConfig()
        {
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryProductCountDTO>().ReverseMap();

            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductCategoryDTO>().ReverseMap();
            CreateMap<ProductDTO, ProductCart>()
                .ForMember(dest => dest.Quantity, opt => opt.Ignore());

            CreateMap<User, UserLoginDTO>().ReverseMap();
            CreateMap<User, UserDetailDTO>().ReverseMap();

            CreateMap<Order, OrderDTO>().ReverseMap();

            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
        }
    }
}
