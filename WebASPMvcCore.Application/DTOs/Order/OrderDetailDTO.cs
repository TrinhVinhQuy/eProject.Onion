using System.ComponentModel.DataAnnotations.Schema;

namespace WebASPMvcCore.Application.DTOs.Order
{
    public class OrderDetailDTO
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quanlity { get; set; }
    }
}
