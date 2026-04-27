using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SzakemberKereso.Models
{
    [Table("residential_addresses")]
    public class ResidentialAddress
    {
        [Column("residential_address_id")]
        public int Id { get; set; }

        [Column("settlement_id")]
        public int SettlementId { get; set; }

        [Column("street_name")]
        [MaxLength(150)]
        public string StreetName { get; set; } = null!;
        [Column("building_number")]
        public int BuildingNumber { get; set; }

        [Column("public_area_type")]
        [MaxLength(16)]
        public string PublicAreaType { get; set; } = null!;

        public Settlement Settlement { get; set; } = null!;
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
