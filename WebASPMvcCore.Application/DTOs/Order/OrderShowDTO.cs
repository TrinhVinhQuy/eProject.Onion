using WebASPMvcCore.Domain.Enums;

namespace WebASPMvcCore.Application.DTOs.Order
{
    public class OrderShowDTO
    {
        public Guid Id { get; set; }
        public DateTime CreateOn { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Town { get; set; }
        public string Address { get; set; }
        public double TotalAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public StatusProcessing StatusProcessing { get; set; }
        public string FullName { get; set; }
    }
}
