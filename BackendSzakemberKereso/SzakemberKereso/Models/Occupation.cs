using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SzakemberKereso.Models
{
    [Table("occupations")]
    public class Occupation
    {
        [Key]
        [Column("feor_id")]
        public int Id { get; set; }

        [Column("name")]
        [StringLength(256)]
        public string Name { get; set; } = null!;

        [Column("description")]
        [StringLength(256)]
        public string Description { get; set; } = null!;

        public virtual ICollection<ExpertSpecialty> ExpertSpecialties { get; set; } = new List<ExpertSpecialty>();
    }
}
