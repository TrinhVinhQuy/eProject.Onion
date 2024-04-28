using System.ComponentModel.DataAnnotations.Schema;

namespace eProject.Domain.Entities
{
    public class Order : BaseEntity
    {
        
        public DateTime CreateOn { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Town { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
        public int? StaffId { get; set; }
        public string InvoiceNumber { get; set; } //vnp_TxnRef Số hoá đơn
        public string TradingCode { get; set; } //vnp_TransactionNo Mã giao dịch tại VnPay
        public bool OrderStatus { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
        
    }
}
