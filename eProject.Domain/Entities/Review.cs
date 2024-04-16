using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProject.Domain.Entities
{
    public class Review : BaseEntity
    {
        public string ContentReview { get; set; }
        public int Rating { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
