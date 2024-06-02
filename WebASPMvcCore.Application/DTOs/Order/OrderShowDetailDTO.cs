namespace WebASPMvcCore.Application.DTOs.Order
{
    public class OrderShowDetailDTO
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quanlity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductName { get; set; }
        public string ProductLink { get; set; }
    }
}
