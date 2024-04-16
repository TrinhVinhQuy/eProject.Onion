using System.ComponentModel.DataAnnotations;

namespace eProject.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaLink { get; set; }
        public string MetaImage { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
