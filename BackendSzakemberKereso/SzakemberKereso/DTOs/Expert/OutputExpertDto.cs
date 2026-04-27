using SzakemberKereso.Models;
using SzakemberKereso.DTOs.Settlement;
using SzakemberKereso.DTOs.ExpertSpecialty;

namespace SzakemberKereso.DTOs.Expert
{
    public class OutputExpertDto
    {
        public int UserId { get; set; }
        public string? Biography { get; set; }
        public string? CompanyPhoneNumber { get; set; }
        public string? CompanyEmail { get; set; }

        //flattened User
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string[] Roles { get; set; } = Array.Empty<string>();
        
        public virtual OutputSettlementDto? WorkLocation { get; set; }
        public virtual ICollection<OutputExpertSpecialtyDto> ExpertSpecialties { get; set; } = new List<OutputExpertSpecialtyDto>();
    }
}
