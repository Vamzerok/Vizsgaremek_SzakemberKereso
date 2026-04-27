using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SzakemberKereso.Models
{
    [Table("expert_specialties")]
    public class ExpertSpecialty
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("expert_id")]
        public int ExpertId { get; set; }
        [Column("occupation_id")]
        public int OccupationId { get; set; }

        public virtual Expert Expert { get; set; } = null!;
        public virtual Occupation Occupation { get; set; } = null!;
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
