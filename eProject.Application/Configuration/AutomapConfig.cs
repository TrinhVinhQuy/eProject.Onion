using AutoMapper;
using eProject.Application.DTOs.Category;
using eProject.Application.DTOs.Role;
using eProject.Domain.Entities;

namespace eProject.Application.Configuration
{
    public class AutomapConfig : Profile
    {
        public AutomapConfig()
        {
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
