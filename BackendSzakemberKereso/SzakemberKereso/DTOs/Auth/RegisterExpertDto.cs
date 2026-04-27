using System.ComponentModel.DataAnnotations;

namespace SzakemberKereso.DTOs.Auth
{
    public class RegisterExpertDto
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

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string CompanyEmail { get; set; } = null!;

        [Required]
        [MaxLength(32)]
        public string CompanyPhoneNumber { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue)]
        public int WorkLocationId { get; set; }

        public string? Biography { get; set; }
    }
}
