using SzakemberKereso.DTOs.Pricing;
using SzakemberKereso.DTOs.ResidentialAddress;
using SzakemberKereso.DTOs.Service;
using SzakemberKereso.DTOs.TimeInterval;
using SzakemberKereso.DTOs.User;
using SzakemberKereso.Models;

namespace SzakemberKereso.DTOs.Job
{
    public class OutputJobDto
    {
        public int Id { get; set; }
        public JobStatus Status { get; set; }
        public JobStatus? CancelledFromStatus { get; set; }

        public int InitiatingUserId { get; set; }
        public OutputUserDto? InitiatingUser { get; set; }

        public int ServiceId { get; set; }
        public OutputServiceDto? Service { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public OutputResidentialAddressDto? Location { get; set; }

        public PricingDto? Pricing { get; set; }
        public float? Rating { get; set; }

        public ICollection<OutputTimeIntervalDto> UserAvailability { get; set; } = new List<OutputTimeIntervalDto>();
        public ICollection<OutputTimeIntervalDto> OfferedAppointments { get; set; } = new List<OutputTimeIntervalDto>();
    }
}
