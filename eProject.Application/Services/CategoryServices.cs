using AutoMapper;
using eProject.Application.Abstracts;
using eProject.Application.DTOs.Category;
using eProject.Domain.Abstracts;
using eProject.Domain.Entities;
using System;
using System.Collections;
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
        private readonly IRepositoryBase<Product> _productRepository;
        private readonly IMapper _mapper;
        public CategoryServices(IRepositoryBase<Category> categoryRepository, IRepositoryBase<Product> productRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
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
        
        public async Task<IEnumerable<CategoryProductCountDTO>> GetCountProductCategoryAsync()
        {
            var _cate = await _categoryRepository.GetAllAsync();
            var _product = await _productRepository.GetAllAsync();
            var listCate = _mapper.Map<IEnumerable<CategoryProductCountDTO>>(_cate);
            foreach(var cate in listCate)
            {
                cate.CountProduct = _product.Where(x=>x.CategoryId == cate.Id).Count();
            }
            return listCate;
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
