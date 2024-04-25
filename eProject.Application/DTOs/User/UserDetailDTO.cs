using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProject.Application.DTOs.User
{
    public class UserDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public int? Province { get; set; }
        public int? District { get; set; }
        public int? Town { get; set; }
        public bool IsActive { get; set; }
    }
}
