using Microsoft.AspNetCore.Identity;

namespace WebASPMvcCore.Domain.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string? Fullname { get; set; }
        public string? MobilePhone { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Town { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }

    }
}
