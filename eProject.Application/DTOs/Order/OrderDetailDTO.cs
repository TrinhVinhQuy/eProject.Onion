using eProject.Application.DTOs.Product;

namespace eProject.Application.DTOs.Order
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Province { get; set; }
        public int District { get; set; }
        public int Town { get; set; }
        public List<ProductCart> Products { get; set; }
    }
}
