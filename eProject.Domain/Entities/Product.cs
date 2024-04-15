using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProject.Domain.Entities
{
    public class Product : BaseEntity
    {
        public required string MetaTitle { get; set; }
        public required string MetaKeywords { get; set; }
        public required string MetaDescription { get; set; }
        public required string MetaLink { get; set; }
        public required string MetaImage { get; set; }
    }
}
    