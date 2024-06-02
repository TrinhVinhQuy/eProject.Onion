using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebASPMvcCore.Application.DTOs.Product
{
    public class ProductViewDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public Guid CategoryId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
