using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SzakemberKereso.Models
{
    [Table("settlements")]
    public class Settlement
    {
        [Column("settlement_id")]
        public int Id { get; set; }
        [Column("postal_code")]
        public int PostalCode { get; set; }
        [Column("name")]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [Column("county_name")]
        [MaxLength(100)]
        public string CountyName { get; set; } = null!;

        public ICollection<ResidentialAddress> Addresses { get; set; } = new List<ResidentialAddress>();
    }
}
