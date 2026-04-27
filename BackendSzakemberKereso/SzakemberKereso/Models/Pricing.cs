using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SzakemberKereso.Models
{
    public enum PricingType
    {
        Fixed = 0,
        FixedAndUnitBased = 1
    }

    [Table("pricing")]
    public class Pricing
    {
        [Key]
        [Column("pricing_id")]
        public int Id { get; set; }

        [Column("pricing_type")]
        public PricingType PricingType { get; set; }

        [Column("fixed_price", TypeName = "decimal(18,2)")]
        public decimal FixedPrice { get; set; }

        [Column("unit_price", TypeName = "decimal(18,2)")]
        public decimal? UnitPrice { get; set; }

        [Column("unit_name")]
        [StringLength(64)]
        public string? UnitName { get; set; }
    }
}
