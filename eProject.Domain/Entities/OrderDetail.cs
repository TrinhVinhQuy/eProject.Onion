using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace eProject.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        [ForeignKey(nameof(User))]
        public int ProductId { get; set; }
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public int Quanlity { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
