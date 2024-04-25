using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProject.Domain.Entities
{
    public class ResetPassword: BaseEntity
    {
        public Guid Code { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? Password { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
