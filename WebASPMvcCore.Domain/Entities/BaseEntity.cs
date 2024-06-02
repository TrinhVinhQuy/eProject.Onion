using System.ComponentModel.DataAnnotations;

namespace WebASPMvcCore.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
