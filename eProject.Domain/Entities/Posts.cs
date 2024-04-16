using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProject.Domain.Entities
{
    public class Posts : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaLink { get; set; }
        public string MetaImage { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
