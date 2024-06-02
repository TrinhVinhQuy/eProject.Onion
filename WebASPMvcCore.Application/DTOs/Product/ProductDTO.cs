
namespace WebASPMvcCore.Application.DTOs.Product
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public int Quantity { get; set; }
        public int SoldItem { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaLink { get; set; }
        public string MetaImage { get; set; }
        public string CategoryId{ get; set; }
        public string CategoryName { get; set; }
        public string CategoryMetaLink { get; set; }
        public DateTime CreateOn { get; set; }
        public bool IsActive { get; set; }
    }
}
