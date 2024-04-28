﻿using AutoMapper;
using eProject.Application.DTOs.Category;
using eProject.Application.DTOs.Order;
using eProject.Application.DTOs.OrderDetail;
using eProject.Application.DTOs.Product;
using eProject.Application.DTOs.ResetPassword;
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
            CreateMap<Product, ProductCart>()
                .ForMember(dest => dest.Quantity, opt => opt.Ignore())
                .ForMember(dest => dest.Price, opt => opt.Ignore())
                .ForMember(dest => dest.Discount, opt => opt.Ignore());

            CreateMap<User, UserLoginDTO>().ReverseMap();
            CreateMap<User, UserDetailDTO>().ReverseMap();

            CreateMap<Order, OrderDTO>().ReverseMap()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
            CreateMap<Order, OrderDetailDTO>().ReverseMap();
            CreateMap<User, OrderDetailDTO>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Province, opt => opt.Ignore())
                .ForMember(dest => dest.District, opt => opt.Ignore())
                .ForMember(dest => dest.Town, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.Ignore());

            CreateMap<OrderDetail, OrderDetailByOrderIdDTO>().ReverseMap();

            CreateMap<ResetPassword, ResetPasswordDTO>().ReverseMap();
        }
    }
}
