using System.ComponentModel.DataAnnotations;

namespace SzakemberKereso.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(32)]
        public string FirstName { get; set; } = null!;
        [Required]
        [MaxLength(32)]
        public string LastName { get; set; } = null!;
    }
}
