using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SzakemberKereso.Models
{
    [Table("services")]
    public class Service
    {
        [Key]
        [Column("service_id")]
        public int Id { get; set; }

        [Column("name")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        [MaxLength(512)]
        public string? Description { get; set; }

        [Column("pricing_id")]
        public int PricingId { get; set; }

        [Column("expert_specialties_id")]
        public int ExpertSpecialtyId { get; set; }


        [ForeignKey(nameof(ExpertSpecialtyId))]
        public virtual ExpertSpecialty ExpertSpecialty { get; set; } = null!;

        [ForeignKey(nameof(PricingId))]
        public virtual Pricing Pricing { get; set; } = null!;

        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
