using SzakemberKereso.DTOs.Pricing;
using SzakemberKereso.DTOs.TimeInterval;

namespace SzakemberKereso.DTOs.Job
{
    public class MakeOfferDto
    {
        public PricingDto Pricing { get; set; } = null!;
        public List<InputTimeIntervalDto> OfferedTimeIntervals { get; set; } = new();
    }
}
