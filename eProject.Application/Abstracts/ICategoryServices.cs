using eProject.Application.DTOs.Category;
using eProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProject.Application.Abstracts
{
    public interface ICategoryServices
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO> GetByIdAsync(int id);
        Task<IEnumerable<CategoryProductCountDTO>> GetCountProductCategoryAsync();
    }
}
