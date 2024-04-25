using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProject.Application.DTOs.ResetPassword
{
    public class ResetPasswordDTO
    {
        public int Id { get; set; }
        public Guid Code { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
