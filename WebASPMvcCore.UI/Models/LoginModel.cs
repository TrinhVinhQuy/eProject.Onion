using System.ComponentModel.DataAnnotations;

namespace WebASPMvcCore.UI.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username must be not empty")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password must be not empty")]
        [MinLength(3, ErrorMessage = "Password must be greater than 3 characters")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
        public string Captcha { get; set; }
    }
}
