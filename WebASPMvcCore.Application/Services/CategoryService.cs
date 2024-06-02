using AutoMapper;
using WebASPMvcCore.Application.Abstracts;
using WebASPMvcCore.Application.DTOs.Category;
using WebASPMvcCore.Domain.Abstracts;

namespace WebASPMvcCore.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CategoryDTO>> GetAllAsync()
        {
            var category = await _unitOfWork.CategoryRepository.GetAllCategoryAsync();
            return _mapper.Map<List<CategoryDTO>>(category);
        }

        public async Task<List<CategoryProductCountDTO>> GetAllCateProductCount()
        {
            var result = await _unitOfWork.CategoryRepository.GetAllCateProductCount<CategoryProductCountDTO>();
            result = result.OrderByDescending(x => x.ProductTotal);
            return _mapper.Map<List<CategoryProductCountDTO>>(result);
        }

        public async Task<Guid> GetCategoryIdByUrlAsync(string url)
        {
            return await _unitOfWork.CategoryRepository.GetCategoryIdByUrlAsync(url);
        }
    }
}
