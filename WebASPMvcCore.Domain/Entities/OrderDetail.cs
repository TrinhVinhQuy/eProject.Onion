
using System.ComponentModel.DataAnnotations.Schema;

namespace WebASPMvcCore.Domain.Entities
{
    public class OrderDetail: BaseEntity
    {
        public Guid ProductId { get; set; }
        [ForeignKey(nameof(Order))]
        public Guid OrderId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quanlity { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
