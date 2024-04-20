using AutoMapper;
using eProject.Application.Abstracts;
using eProject.Application.DTOs.Product;
using eProject.Domain.Abstracts;
using eProject.Domain.Entities;

namespace eProject.Application.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IRepositoryBase<Product> _productRepository;
        private readonly IRepositoryBase<ProductImage> _productImageRepository;
        private readonly IRepositoryBase<Category> _categoryRepository;
        private readonly IMapper _mapper;
        public ProductServices(IRepositoryBase<Product> productServices, IRepositoryBase<ProductImage> productImageRepository, IRepositoryBase<Category> categoryRepository, IMapper mapper)
        {
            _productRepository = productServices;
            _productImageRepository = productImageRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var _pro = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(_pro);
        }
        public async Task<IEnumerable<ProductCategoryDTO>> GetByCategoryAsync()
        {
            var _pro = await _productRepository.GetAllAsync();
            var _proImage = await _productImageRepository.GetAllAsync();
            var _cate = await _categoryRepository.GetAllAsync();
            var _proResult = _mapper.Map<IEnumerable<ProductCategoryDTO>>(_pro);
            foreach (var item in _proResult)
            {
                item.CategoryName = _cate.First(x => x.Id == item.CategoryId).Name;
                var images = _proImage.Where(x => x.ProductId == item.Id)
                                   .Select(x => new ProductImgaeDTO
                                   {
                                       Id = x.Id,
                                       Image = x.Image
                                   })
                                   .ToList();
                item.Images = images;
                item.CategoryMetaLink = _cate.First(x => x.Id == item.CategoryId).MetaLink;
            }
            return _proResult;
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            var _pro = await _productRepository.GetAllAsync();
            return _mapper.Map<ProductDTO>(_pro.First(x => x.Id == id));
        }

        public Task<Product> InsertAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
