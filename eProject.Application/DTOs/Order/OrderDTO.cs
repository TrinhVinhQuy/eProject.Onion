using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProject.Application.DTOs.Order
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string? NameStaff { get; set; }
        public bool Status { get; set; }
        public DateTime CreateOn { get; set; }
        public bool OrderStatus { get; set; }
        public string TradingCode { get; set; }
        public bool? IsActive { get; set; }
    }
}
