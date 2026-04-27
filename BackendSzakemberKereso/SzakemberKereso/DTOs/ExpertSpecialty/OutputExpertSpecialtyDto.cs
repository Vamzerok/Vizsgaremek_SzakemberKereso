using SzakemberKereso.DTOs.Service;
using SzakemberKereso.DTOs.Occupation;

namespace SzakemberKereso.DTOs.ExpertSpecialty
{
    public class OutputExpertSpecialtyDto
    {
        public int Id { get; set; }
        public int OccupationId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public virtual ICollection<OutputServiceDto> Services { get; set; } = new List<OutputServiceDto>();
    }
}
