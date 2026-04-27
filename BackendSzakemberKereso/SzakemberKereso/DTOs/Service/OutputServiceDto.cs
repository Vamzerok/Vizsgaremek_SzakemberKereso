using SzakemberKereso.DTOs.Pricing;

namespace SzakemberKereso.DTOs.Service
{
    public class OutputServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ExpertSpecialtyId { get; set; }
        public int PricingId { get; set; }
        public PricingDto Pricing { get; set; } = null!;
        public string ExpertName { get; set; } = string.Empty;
        public int ExpertUserId { get; set; }
    }
}
