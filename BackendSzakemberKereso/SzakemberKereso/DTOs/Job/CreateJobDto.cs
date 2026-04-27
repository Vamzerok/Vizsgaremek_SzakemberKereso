using System.ComponentModel.DataAnnotations;
using SzakemberKereso.DTOs.ResidentialAddress;
using SzakemberKereso.DTOs.TimeInterval;

namespace SzakemberKereso.DTOs.Job
{
    public class CreateJobDto
    {
        public int ServiceId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public InputResidentialAddressDto Location { get; set; } = null!;
        public List<InputTimeIntervalDto> AvailableTimeIntervals { get; set; } = new();
    }
}
