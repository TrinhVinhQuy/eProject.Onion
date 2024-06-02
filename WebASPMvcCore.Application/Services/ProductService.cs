using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using WebASPMvcCore.Application.Abstracts;
using WebASPMvcCore.Application.DTOs;
using WebASPMvcCore.Application.DTOs.Product;
using WebASPMvcCore.Domain.Abstracts;
using WebASPMvcCore.Domain.Entities;

namespace WebASPMvcCore.Application.Services
{
    public class ProductService : IProductService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<ResponseDatatable<ProductDTO>> GetBooksByPaginationAsync(RequestDatatable request)
        {
            int totalRecords = 0;
            IEnumerable<ProductDTO> products;

            (products, totalRecords) = await _unitOfWork.ProductRepository.GetProductsByPaginationAsync<ProductDTO>(request.Page,
                                                                                                    request.PageSize,
                                                                                                    request.Keyword,
                                                                                                    request.CategoryId,
                                                                                                    request.PriceMin,
                                                                                                    request.PriceMax);

            return new ResponseDatatable<ProductDTO>
            {
                RecordsTotal = totalRecords,
                Data = products
            };
        }

        public async Task<ProductPriceDTO> GetProductPriceMinMaxAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetProductsByListCodeAsync();

            return new ProductPriceDTO
            {
                PriceMax = products.OrderByDescending(x => x.Price).First().Price,
                PriceMin = products.OrderBy(x => x.Price).First().Price,
            };
        }

        public async Task<IEnumerable<ProductDTO>> GetProductTopAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetProductsByListCodeAsync();
            products = products.Where(x => x.SoldItem > 0).OrderByDescending(x => x.SoldItem).Take(4);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDetailDTO> GetProductDetailAsync(string url)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByUrlAsync(url);

            if (product == null)
            {
                return null;
            }

            var category = await _unitOfWork.CategoryRepository.GetCategoryByIdAsync(product.CategoryId);

            var result = new ProductDetailDTO();
            result = _mapper.Map<ProductDetailDTO>(product);
            result.Images = product.ProductImages.Select(pi => pi.Image).ToList();
            result.CategoryName = category.Name;
            result.CategoryMetaLink = category.MetaLink;

            return result;
        }

        public async Task<IEnumerable<ProductCartDTO>> GetAllProductCartsAsync()
        {
            var product = await _unitOfWork.ProductRepository.GetProductsByListCodeAsync();
            return _mapper.Map<IEnumerable<ProductCartDTO>>(product);
        }

        public async Task<ProductCartDTO> GetProductCartByIdAsync(Guid Id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(Id);
            return _mapper.Map<ProductCartDTO>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var product = await _unitOfWork.ProductRepository.GetProductsByListCodeAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(product);
        }

        public async Task<bool> CreateAsync(Product product, List<IFormFile> Images)
        {
            if (product == null) return false;

            var proImages = new List<ProductImage>();

            try
            {
                product.Id = Guid.NewGuid();

                foreach (var item in Images)
                {
                    var proImage = new ProductImage
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        Image = await SaveImageAsync(item, "product")
                    };
                    proImages.Add(proImage);
                }
                product.MetaImage = proImages.First().Image;
                product.ProductImages = proImages;

                await _unitOfWork.BeginTransaction();

                await _unitOfWork.ProductRepository.SaveAsync(product);

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                // Xóa các ảnh đã lưu trong trường hợp có lỗi
                foreach (var proImage in proImages)
                {
                    DeleteImageAsync(proImage.Image);
                }
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
            return true;
        }
        
        public async Task<ProductDetailDTO> GetProductDetailByIdAsync(Guid Id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(Id);

            if (product == null)
            {
                return null;
            }
            return _mapper.Map<ProductDetailDTO>(product);
        }
        
        public async Task<bool> IsActiveAsync(Guid Id, bool IsActive)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(Id);
                if (product == null) return false;

                product.IsActive = IsActive;

                await _unitOfWork.BeginTransaction();

                await _unitOfWork.ProductRepository.SaveAsync(product);

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(); 
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductAsync()
        {
            var product = await _unitOfWork.ProductRepository.GetAllProductsAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(product);
        }

        public async Task<bool> GetProductUpdateAsync(ProductUpdateDTO model)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(model.Id);
                if (product == null) return false;

                _mapper.Map(model,product);

                await _unitOfWork.BeginTransaction();

                await _unitOfWork.ProductRepository.SaveAsync(product);

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
            return true;
        }
        // Hàm xóa ảnh
        private void DeleteImageAsync(string imagePath)
        {
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
        // Hàm lưu ảnh
        private async Task<string> SaveImageAsync(IFormFile image, string folder)
        {
            // Tạo tên tệp duy nhất cho hình ảnh
            var fileName = image.FileName;
            var imagePath = Path.Combine(_environment.WebRootPath, folder, fileName);

            // Lưu hình ảnh vào thư mục wwwroot
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Trả về đường dẫn tương đối của hình ảnh
            return "/" + Path.Combine(folder, fileName).Replace("\\", "/");
        }
    }
}
