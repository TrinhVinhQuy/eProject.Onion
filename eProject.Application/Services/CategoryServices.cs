using AutoMapper;
using eProject.Application.Abstracts;
using eProject.Application.DTOs.Category;
using eProject.Domain.Abstracts;
using eProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace eProject.Application.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IRepositoryBase<Category> _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryServices(IRepositoryBase<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task DeleteAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var _cate = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(_cate);
        }

        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            var _cateDetail = await _categoryRepository.GetByIdAsync(id);
            return _mapper.Map<CategoryDTO>(_cateDetail);
        }

        public async Task<Category> InsertAsync(Category category)
        {
            await _categoryRepository.InsertAsync(category);
            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            await _categoryRepository.UpdateAsync(category);
        }
    }
}
