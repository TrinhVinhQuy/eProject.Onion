
namespace eProject.Application.DTOs.Product
{
    public class ProductCart
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public string MetaLink { get; set; }
        public string MetaImage { get; set; }
        public int Quantity { get; set; }
    }
}
