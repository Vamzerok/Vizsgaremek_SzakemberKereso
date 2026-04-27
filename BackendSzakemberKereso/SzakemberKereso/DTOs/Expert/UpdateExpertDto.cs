using System.ComponentModel.DataAnnotations;

namespace SzakemberKereso.DTOs.Expert
{
    public class UpdateExpertDto
    {
        public string? Biography { get; set; }

        [Required]
        [MaxLength(32)]
        public string CompanyPhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string CompanyEmail { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue)]
        public int WorkLocationId { get; set; }
    }
}
