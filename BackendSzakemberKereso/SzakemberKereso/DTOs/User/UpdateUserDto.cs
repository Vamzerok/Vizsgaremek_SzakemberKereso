using System.ComponentModel.DataAnnotations;

namespace SzakemberKereso.DTOs.User
{
    public class UpdateUserDto
    {
        [Required]
        [MaxLength(32)]
        public string FirstName { get; set; } = null!;
        [Required]
        [MaxLength(32)]
        public string LastName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }
}
