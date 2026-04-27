using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SzakemberKereso.Models
{
    public enum JobStatus
    {
        Pending,
        Offered,
        Accepted,
        Completed,
        Cancelled
    }
    [Table("jobs")]
    public class Job
    {
        [Key]
        [Column("job_id")]
        public int Id { get; set; }

        [Column("status")]
        public JobStatus Status { get; set; }

        //pending
        [Column("initiating_user_id")]
        public int InitiatingUserId { get; set; }
        [Column("service_id")]
        public int ServiceId { get; set; }

        [Column("location_id")]
        public int LocationId { get; set; }

        [Column("title")]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [ForeignKey(nameof(ServiceId))]
        [InverseProperty("Jobs")]
        public virtual Service? Service { get; set; }

        [ForeignKey(nameof(InitiatingUserId))]
        [InverseProperty("InitiatedJobs")]
        public virtual User InitiatingUser { get; set; } = null!;

        //offered
        [Column("pricing_id")]
        public int? PricingId { get; set; }

        [ForeignKey(nameof(PricingId))]
        public virtual Pricing? Pricing { get; set; }

        //cancelled
        [Column("cancelled_from_status")]
        public JobStatus? CancelledFromStatus { get; set; }

        //completed
        [Column("rating")]
        public float? Rating { get; set; }

        //navigation properties
        [ForeignKey(nameof(LocationId))]
        [InverseProperty("Jobs")]
        public virtual ResidentialAddress Location { get; set; } = null!;

        public virtual ICollection<TimeInterval> TimeIntervals { get; set; } = new List<TimeInterval>();
    }
}
