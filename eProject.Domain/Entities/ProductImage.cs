using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProject.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public string Image { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
