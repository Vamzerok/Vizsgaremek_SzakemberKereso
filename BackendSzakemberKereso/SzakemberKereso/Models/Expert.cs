using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SzakemberKereso.Models
{
    [Table("experts")]
    public class Expert
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("biography")]
        [StringLength(512)] //feels generous
        public string? Biography { get; set; }

        [Column("company_phone_number")]
        [StringLength(32)]
        public string CompanyPhoneNumber { get; set; } = null!;

        [Column("company_email")]
        [StringLength(256)]
        public string CompanyEmail { get; set; } = null!;

        [Column("work_location_id")]
        public int WorkLocationId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Expert")]
        public virtual User User { get; set; } = null!;

        public virtual Settlement WorkLocation { get; set; } = null!;

        public virtual ICollection<ExpertSpecialty> ExpertSpecialties { get; set; } = new List<ExpertSpecialty>();
    }
}
