using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProject.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string? Address { get; set; }
        public int? Province { get; set; }
        public int? District { get; set; }
        public int? Town { get; set; }

        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
