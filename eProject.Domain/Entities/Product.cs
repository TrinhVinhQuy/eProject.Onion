using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eProject.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaLink { get; set; }
        public string MetaImage { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
